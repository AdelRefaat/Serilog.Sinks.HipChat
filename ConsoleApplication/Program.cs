using Serilog;
using Serilog.Events;
using Serilog.Sinks.HipChat;

namespace ConsoleApplication
{
    class Program
    {
        static void Main()
        {
            const string token = "Token Here";
            const string roomId = "RoomId Here";
            var log = new LoggerConfiguration().WriteTo.HipChat(token, roomId , LogEventLevel.Verbose, 1, 1).CreateLogger();
            log.Error("Hello !");
        }
    }
}
