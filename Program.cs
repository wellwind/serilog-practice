using System;
using Serilog;
using Serilog.Context;
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
                // 要使用LogContext.PushProperty, Enrich.FromLogcontext()為必要
                .Enrich.FromLogContext()
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

                // 加入特定property
                var specialLog = log.ForContext("Customer", "Mike");
                specialLog.Warning("A very rich man! balance: {balance} millions", 100);

                // 以某個Class為Property (property name: SourceProperty)
                var contextLog = log.ForContext<SomeContext>();
                contextLog.Warning("context log");

                // 動態加入property
                using (LogContext.PushProperty("OrderNumber", "ABC123"))
                {
                    using (LogContext.PushProperty("TransactionId", "T12345"))
                    {
                        log.Debug("this log has TransactionId");

                        using (LogContext.PushProperty("OrderNumber", "ABC123"))
                        {
                            log.Debug("this log has OrderNumber");
                        }
                    }

                    log.Debug("this log has no TransactionId");
                }

                log.Debug("this log has no OrderNumber");
            }
        }
    }

    public class SomeContext
    {
    }
}