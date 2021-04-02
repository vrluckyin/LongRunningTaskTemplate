using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LongRunningTaskTemplate
{
    public class SendEmailNotificationTask : JobServiceTask
    {
        public SendEmailNotificationTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            JobTaskName = nameof(SendEmailNotificationTask);
        }
        public async override Task<Dictionary<string, object>> Execute()
        {
            //Do your job
            for (var i = 0; i < 1000; i++)
            {
                await Task.Delay(10);
            }
            return null;
        }
    }
}
