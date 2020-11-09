using AutoMapper;
using IPAddressProccessor.API.Data;
using IPAddressProccessor.API.Data.Entities;
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
    public class DatabaseIPDetailsService : IIPDetailsService
    {
        private readonly IIPAddressesRepository repository;
        private readonly ILogger<DatabaseIPDetailsService> logger;
        private readonly IMapper mapper;
        private readonly ExternalAPIIPDetailsService externalAPIIPDetailsService;

        private static readonly SemaphoreSlim ipDetailsDbSemaphore = new SemaphoreSlim(1, 1);

        public DatabaseIPDetailsService(
            IIPAddressesRepository repository,
            ILogger<DatabaseIPDetailsService> logger,
            IMapper mapper,
            ExternalAPIIPDetailsService externalAPIIPDetailsService
            )
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
            this.externalAPIIPDetailsService = externalAPIIPDetailsService;
        }

        public async Task<IPAddressModel> Get(string ip)
        {
            return await this.GetIPDetailsFromDatabase(ip, ipDetailsDbSemaphore, () => this.externalAPIIPDetailsService.Get(ip));
        }

        private async Task<IPAddressModel> GetIPDetailsFromDatabase(string ip, SemaphoreSlim semaphore, Func<Task<IPAddressModel>> getFromExternalSourceFunc)
        {
            this.logger.LogInformation($"Attempting to find ip: {ip} in database...");
            var ipDetails = await this.repository.GetIPAddressDetailsAsync(ip);

            if (ipDetails != null)
            {
                this.logger.LogInformation($"Found details for {ip} in database.");
                return this.mapper.Map<IPAddressModel>(ipDetails);
            }

            try
            {
                await semaphore.WaitAsync();

                //Re-check to make sure that a value has not been added while waiting to enter the semaphore
                ipDetails = await this.repository.GetIPAddressDetailsAsync(ip);

                if (ipDetails != null)
                {
                    return this.mapper.Map<IPAddressModel>(ipDetails);
                }

                //If not in database from other source
                var ipDetailsFromExternalSource = await getFromExternalSourceFunc();

                //Put data in database
                this.repository.Add(this.mapper.Map<IPAddress>(ipDetailsFromExternalSource));
                if(await this.repository.SaveChangesAsync())
                {
                    return ipDetailsFromExternalSource;
                }
                else
                {
                    throw new Exception("Failed to save ip details to database");
                }

            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
