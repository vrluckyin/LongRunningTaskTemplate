using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LongRunningTaskTemplate
{
    class Program
    {
        private static IConfigurationRoot _configuration = null;
        static void Main(string[] args)
        {
            IConfiguration serilogConfig = new ConfigurationBuilder()
                    .AddJsonFile("serilog.json")
                    .Build();

            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(serilogConfig)
                    .CreateLogger();

            try
            {
                IConfigurationBuilder cbuilder = new ConfigurationBuilder()
                                                .AddJsonFile("appsettings.json");

                _configuration = cbuilder.Build();

                var startup = new Startup(_configuration);
                IServiceProvider provider = startup.ConfigureServices();
                JobService jobService = provider.GetRequiredService<JobService>();
                jobService.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Log.Error(ex, "Failed to start JobService tasks");
            }
        }
    }
}
