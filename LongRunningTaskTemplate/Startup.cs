using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LongRunningTaskTemplate
{
    public class Startup
    {
        private readonly IConfiguration _configuration = null;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(configure =>
            {
                configure.AddSerilog(Log.Logger);
            });

            services.AddScoped<JobService>();
            services.AddSingleton(_configuration);
            return services.BuildServiceProvider();
        }
    }
}
