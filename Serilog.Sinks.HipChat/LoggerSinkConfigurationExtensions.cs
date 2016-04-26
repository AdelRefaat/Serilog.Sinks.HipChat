using System;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.HipChat
{
    public static class LoggerSinkConfigurationExtensions
    {
        public static LoggerConfiguration HipChat(this LoggerSinkConfiguration sinkConfiguration,
            string token, 
            string roomId,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Error,
            int batchSizeLimit = 250, int periodInSeconds = 150/*2.5 Minutes*/,
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
        {
            if (sinkConfiguration == null)
            {
                throw new ArgumentNullException("sinkConfiguration");
            }
            if (outputTemplate == null)
            {
                throw new ArgumentNullException("outputTemplate");
            }
            var hipChatSink = new HipChatSink(token, roomId, restrictedToMinimumLevel, batchSizeLimit, TimeSpan.FromSeconds(periodInSeconds));
            return sinkConfiguration.Sink(hipChatSink, restrictedToMinimumLevel);
        }
    }
}
