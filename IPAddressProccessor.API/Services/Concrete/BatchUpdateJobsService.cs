using AutoMapper;
using IPAddressProccessor.API.Data;
using IPAddressProccessor.API.Data.Entities;
using IPAddressProccessor.API.Models;
using IPAddressProccessor.API.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Concrete
{
    public class BatchUpdateJobsService : IBatchUpdateJobsService
    {
        private readonly IIPAddressesRepository iPAddressesRepository;
        private readonly IMapper mapper;
        private readonly ICacheProvider cacheProvider;
        private readonly ILogger<BatchUpdateJobsService> logger;
        private readonly ExternalAPIIPDetailsService externalAPIIPDetailsService;

        public BatchUpdateJobsService(
            IIPAddressesRepository iPAddressesRepository,
            IMapper mapper,
            ICacheProvider cacheProvider,
            ILogger<BatchUpdateJobsService> logger,
            ExternalAPIIPDetailsService externalAPIIPDetailsService)
        {
            this.iPAddressesRepository = iPAddressesRepository;
            this.mapper = mapper;
            this.cacheProvider = cacheProvider;
            this.logger = logger;
            this.externalAPIIPDetailsService = externalAPIIPDetailsService;
        }

        public async Task<BatchUpdateJobModel> Create(CreateBatchUpdateJobRequest request)
        {
            var model = this.mapper.Map<BatchUpdateJobModel>(request);
            var job = this.mapper.Map<BatchUpdateJob>(model);
            this.iPAddressesRepository.Add(job);

            if (await this.iPAddressesRepository.SaveChangesAsync())
            {
                return this.mapper.Map<BatchUpdateJobModel>(job);
            }
            else
            {
                throw new DbUpdateException();
            }
        }

        public async Task<BatchUpdateJobModel> Get(Guid jobId)
        {
            var job = await this.iPAddressesRepository.GetBatchUpdateJobAsync(jobId);

            if (job == null)
            {
                return null;
            }

            return this.mapper.Map<BatchUpdateJobModel>(job);
        }

        public async Task<bool> DoUpdate()
        {
            var uncompleteJobItems = await this.iPAddressesRepository.GetUncompleteBatchUpdateJobItemsSortedByDateAsync();

            if (uncompleteJobItems != null)
            {
                var batchItems = uncompleteJobItems.Take(10).ToList();


                batchItems.ForEach(i => i.Processing = true);
                if (await this.iPAddressesRepository.SaveChangesAsync())
                {
                    foreach (var item in batchItems)
                    {
                        try
                        {
                            this.logger.LogInformation($"Updating details for ip: {item.IpToUpdate}...");
                            var ipDetails = await this.externalAPIIPDetailsService.Get(item.IpToUpdate);
                            var existingIpDetails = await this.iPAddressesRepository.GetIPAddressDetailsAsync(ipDetails.Ip);

                            if (existingIpDetails == null)
                            {
                                this.iPAddressesRepository.Add(this.mapper.Map<IPAddress>(ipDetails));
                            }
                            else
                            {
                                this.mapper.Map(ipDetails, existingIpDetails);
                            }

                            item.UpdateComplete = true;
                            item.Processing = false;
                            if (item.UpdateJob.JobItems.Where(i => i.UpdateComplete == false).Count() == 0)
                            {
                                item.UpdateJob.Complete = true;
                            }

                            if (await this.iPAddressesRepository.SaveChangesAsync())
                            {
                                this.cacheProvider.SetCache<IPAddressModel>(ipDetails.Ip, ipDetails);
                            }
                        }
                        catch (Exception ex)
                        {
                            item.Processing = false;
                            var saved = await this.iPAddressesRepository.SaveChangesAsync();
                        }
                    }
                }

            }

            return true;

        }

    }
}
