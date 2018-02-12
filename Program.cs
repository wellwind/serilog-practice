using System;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace serilog_practice
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "Demo")
                .Enrich.WithProperty("Server", System.Net.Dns.GetHostName())
                .WriteTo.Console()
                .WriteTo.File("./log/log.txt")
                .WriteTo.File(new JsonFormatter(), "./log/log.json")
                .WriteTo.File(new CompactJsonFormatter(), "./log/log.clef")
                .CreateLogger())
            {
                try
                {
                    throw new Exception("exception test");
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Error Happened: {message}", ex.Message);
                }
                log.Debug("Test");
                log.Information("Hello, Serilog!");
                log.Warning("Goodbye, Serilog.");
                var specialLog = log.ForContext("Customer", "Mike");
                specialLog.Warning("A very rich man! balance: {balance} millions", 100);
            }
        }
    }
}