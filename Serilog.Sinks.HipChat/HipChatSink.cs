using System;
using HipChat.Net;
using HipChat.Net.Http;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.HipChat
{
    internal class HipChatSink : ILogEventSink, IDisposable
    {
        private readonly string _roomId;
        private readonly LogEventLevel _restrictedToMinimumLevel;

        private readonly HipChatClient _client;
        private readonly object _syncRoot = new object();

        public HipChatSink(string token, string roomId, LogEventLevel restrictedToMinimumLevel)
        {
            _roomId = roomId;
            _restrictedToMinimumLevel = restrictedToMinimumLevel;
            _client = new HipChatClient(new ApiConnection(new Credentials(token)));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level < _restrictedToMinimumLevel)
                return;

            lock (_syncRoot)
            {
                var message = logEvent.RenderMessage();
                _client.Rooms.SendNotificationAsync(_roomId, message);
            }
        }

        public void Dispose()
        {
        }
    }
}
