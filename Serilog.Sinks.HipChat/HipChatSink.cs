using System;
using System.Collections.Generic;
using System.Linq;
using HipChat.Net;
using HipChat.Net.Http;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.HipChat
{
    internal class HipChatSink : PeriodicBatchingSink
    {
        private readonly string _roomId;
        private readonly LogEventLevel _restrictedToMinimumLevel;

        private readonly HipChatClient _client;
        private readonly object _syncRoot = new object();

        public HipChatSink(string token, string roomId, LogEventLevel restrictedToMinimumLevel, int batchSizeLimit, TimeSpan period) : base(batchSizeLimit, period)
        {
            _roomId = roomId;
            _restrictedToMinimumLevel = restrictedToMinimumLevel;
            _client = new HipChatClient(new ApiConnection(new Credentials(token)));
        }

        protected override bool CanInclude(LogEvent logEvent)
        {
            return logEvent.Level >= _restrictedToMinimumLevel;
        }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            lock (_syncRoot)
            {
                foreach (var message in events.Select(logEvent => logEvent.RenderMessage()))
                {
                    _client.Rooms.SendNotificationAsync(_roomId, message).Wait();
                }
            }
        }
    }
}
