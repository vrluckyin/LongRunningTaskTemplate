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
        private IServiceScope _serviceScope { get; set; }

        protected ILogger<JobServiceTask> Logger { get; private set; }
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceScope.ServiceProvider;
            }
        }

        public JobServiceTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceScope = _serviceScopeFactory.CreateScope();
            Logger = ServiceProvider.GetService<ILogger<JobServiceTask>>();
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
                    _lock.Release();
                    await Task.Delay(100);
                    Console.WriteLine($"RELEASED {JobTaskName} - {Task.CurrentId}");
                    Logger.LogInformation($"RELEASED {JobTaskName} - {Task.CurrentId}");
                    return result;
                }
                catch (Exception ex)
                {
                    _lock.Release();
                    Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
                    Logger.LogError(ex, $"Runing a task {JobTaskName}");
                }
                finally
                {
                    _serviceScope.Dispose();
                }
            }
            else
            {
                Console.WriteLine($"FAILED {JobTaskName} - {Task.CurrentId}");
                Logger.LogInformation($"FAILED {JobTaskName} - {Task.CurrentId}");
            }
            return null;
        }
    }
}
