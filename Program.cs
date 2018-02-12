using System;
using Serilog;

namespace serilog_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger())
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
            }
        }
    }
}