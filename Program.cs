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
                // Package: Serilog.Enrichers.Environment
                .Enrich.WithMachineName()
                // Package: Serilog.Sinks.Console
                .WriteTo.Console()
                // Package: Serilog.Sinks.File
                .WriteTo.File("./log/log.txt")
                .WriteTo.File(new JsonFormatter(), "./log/log.json")
                // Package: Serilog.Formatting.Compact
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

                var contextLog = log.ForContext<SomeContext>();
                contextLog.Warning("context log");
            }
        }
    }

    public class SomeContext
    {
    }
}