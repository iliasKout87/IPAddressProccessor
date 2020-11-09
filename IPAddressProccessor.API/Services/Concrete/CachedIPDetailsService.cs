using IPAddressProccessor.API.Models;
using IPAddressProccessor.API.Services.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Concrete
{
    public class CachedIPDetailsService : IIPDetailsService
    {
        private readonly ICacheProvider cacheProvider;
        private readonly ILogger<CachedIPDetailsService> logger;
        private readonly DatabaseIPDetailsService databaseIPDetailsService;

        private static readonly SemaphoreSlim ipDetailsSemaphore = new SemaphoreSlim(1, 1);

        public CachedIPDetailsService(
            ICacheProvider cacheProvider,
            ILogger<CachedIPDetailsService> logger,
            DatabaseIPDetailsService databaseIPDetailsService)
        {
            this.cacheProvider = cacheProvider;
            this.logger = logger;
            this.databaseIPDetailsService = databaseIPDetailsService;
        }
        public async Task<IPAddressModel> Get(string ip)
        {
            return await this.GetCachedIPDetails(ip, ipDetailsSemaphore, () => this.databaseIPDetailsService.Get(ip));
        }

        private async Task<IPAddressModel> GetCachedIPDetails(string ip, SemaphoreSlim semaphore, Func<Task<IPAddressModel>> getFromExternalSourceFunc)
        {
            this.logger.LogInformation($"Attempting to find ip: {ip} in cache...");
            var ipDetails = this.cacheProvider.GetFromCache<IPAddressModel>(ip);

            if(ipDetails != null)
            {
                this.logger.LogInformation($"Found details for {ip} in cache");
                return ipDetails;
            }

            try
            {
                await semaphore.WaitAsync();

                //Re-check to make sure that a value has not been added while waiting to enter the semaphore
                ipDetails = this.cacheProvider.GetFromCache<IPAddressModel>(ip);
                if (ipDetails != null)
                {
                    return ipDetails;
                }

                //If not in cache fetch from external source
                ipDetails = await getFromExternalSourceFunc();

                //Put data in cache
                this.cacheProvider.SetCache<IPAddressModel>(ipDetails.Ip, ipDetails);


            }
            finally
            {
                semaphore.Release();
            }

            return ipDetails;
        }
    }
}
