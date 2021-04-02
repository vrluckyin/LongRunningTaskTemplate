using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LongRunningTaskTemplate
{
    public class JobService
    {
        private readonly ILogger _logger = null;
        private readonly IServiceScopeFactory _serviceScopeFactory = null;

        public JobService(ILogger<JobService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Run()
        {
            try
            {

                while (true)
                {
                    List<JobServiceTask> tasks = new List<JobServiceTask>{
                                        new ResendFailuredEmailNotificationTask(_serviceScopeFactory),
                                        new SendEmailNotificationTask(_serviceScopeFactory)
                                        
                                        };


                    tasks.ForEach((t) => t.Run());
                    Task.Delay(100).GetAwaiter().GetResult();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during running tasks");
                throw;
            }
        }
    }
}
