using Serilog;
using Serilog.Events;
using Serilog.Sinks.HipChat;

namespace ConsoleApplication
{
    class Program
    {
        static void Main()
        {
            const string token = "Token goes here";
            const string roomId = "Room Id goes here";
            using (var log = new LoggerConfiguration().WriteTo.HipChat(token, roomId, LogEventLevel.Verbose, 1, 1).CreateLogger())
            {
                log.Error("Hello !");
            }
        }
    }
}
