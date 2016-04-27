using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using Flurl.Http;

namespace Serilog.Sinks.HipChat
{
    internal class HipChatSink : PeriodicBatchingSink
    {
        private readonly string _token;
        private readonly string _roomId;
        private readonly LogEventLevel _restrictedToMinimumLevel;

      
        private readonly object _syncRoot = new object();

        public HipChatSink(string token, string roomId, LogEventLevel restrictedToMinimumLevel, int batchSizeLimit, TimeSpan period) : base(batchSizeLimit, period)
        {
            _token = token;
            _roomId = roomId;
            _restrictedToMinimumLevel = restrictedToMinimumLevel;
           
        }

        protected override bool CanInclude(LogEvent logEvent)
        {
            return logEvent.Level >= _restrictedToMinimumLevel;
        }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            var apiUrl = string.Format("https://api.hipchat.com/v2/room/{0}/notification", _roomId);
            lock (_syncRoot)
            {
                foreach (var message in events.Select(logEvent => logEvent.RenderMessage()))
                {
                    apiUrl.WithOAuthBearerToken(_token).PostJsonAsync(new {message}).Wait();
                }
            }
        }
    }
}
