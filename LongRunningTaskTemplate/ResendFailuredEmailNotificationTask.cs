using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LongRunningTaskTemplate
{
    public class ResendFailuredEmailNotificationTask : JobServiceTask
    {
        public ResendFailuredEmailNotificationTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            JobTaskName = $">>> {nameof(ResendFailuredEmailNotificationTask)}";
        }
        public async override Task<Dictionary<string, object>> Execute()
        {
            //Do your job
            for (var i = 0; i < 30; i++)
            {
                await Task.Delay(100);
            }
            return null;
        }
    }
}
