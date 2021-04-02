using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LongRunningTaskTemplate
{
    public abstract class JobServiceTask
    {
        protected string JobTaskName { get; set; }
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private IServiceScopeFactory _serviceScopeFactory { get; set; }
        private Lazy<IServiceScope> _serviceScope { get; set; }

        private ILogger<JobServiceTask> _logger = null;
        protected ILogger<JobServiceTask> Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = ServiceProvider.GetService<ILogger<JobServiceTask>>();
                }
                return _logger;
            }
        }
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceScope.Value.ServiceProvider;
            }
        }

        public JobServiceTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceScope = new Lazy<IServiceScope>(() =>
            {
                return _serviceScopeFactory.CreateScope();
            });
        }

        public abstract Task<Dictionary<string, object>> Execute();

        public async Task<Dictionary<string, object>> Run()
        {
            if (_lock.Wait(100))
            {
                try
                {
                    Console.WriteLine($"<<<< LOCKED {JobTaskName} - {Task.CurrentId}");
                    Logger.LogInformation($"<<<< LOCKED {JobTaskName} - {Task.CurrentId}");
                    var result = await Execute();
                    await Task.Delay(100);
                    Console.WriteLine($"RELEASED {JobTaskName} - {Task.CurrentId}");
                    Logger.LogInformation($"RELEASED {JobTaskName} - {Task.CurrentId}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
                    Logger.LogError(ex, $"Runing a task {JobTaskName}");
                }
                finally
                {
                    _lock.Release();
                    if (_serviceScope.IsValueCreated)
                    {
                        _serviceScope.Value.Dispose();
                    }
                }
            }
            /*Else part is only for debug*/
            //else
            //{
            //    Console.WriteLine($"FAILED {JobTaskName} - {Task.CurrentId}");
            //    Logger.LogInformation($"FAILED {JobTaskName} - {Task.CurrentId}");
            //}
            return null;
        }
    }
}
