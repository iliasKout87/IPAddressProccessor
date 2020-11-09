using IPAddressProccessor.API.Services.Abstract;
using IPAddressProccessor.API.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.HostedServices
{
    public class BatchUpdateJobWorker : IHostedService, IDisposable
    {
        private readonly ILogger<BatchUpdateJobWorker> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer timer;

        public BatchUpdateJobWorker(
            ILogger<BatchUpdateJobWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(async o => {
                this.logger.LogInformation("Background worker started");
                using (var scope = this.scopeFactory.CreateScope())
                {
                    var batchUpdateJobsService = scope.ServiceProvider.GetService<IBatchUpdateJobsService>();
                    var batchUpdate = await batchUpdateJobsService.DoUpdate();
                    var batchUpdateResult = batchUpdate ? "true" : "false";
                    this.logger.LogInformation($"Batch update result: {batchUpdateResult}");
                    //this.StopAsync(new System.Threading.CancellationToken());
                }               
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Background worker stopped");

            return Task.CompletedTask;
        }
    }
}
