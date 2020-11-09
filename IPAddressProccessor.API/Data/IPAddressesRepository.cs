using IPAddressProccessor.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data
{
    public class IPAddressesRepository : IIPAddressesRepository
    {
        private readonly IPAddressesContext context;
        private readonly ILogger<IPAddressesRepository> logger;

        public IPAddressesRepository(
            IPAddressesContext context,
            ILogger<IPAddressesRepository> logger
            )
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<IPAddress> GetIPAddressDetailsAsync(string ip)
        {
            this.logger.LogInformation($"Getting details for ip address {ip} from database.");

            IQueryable<IPAddress> query = this.context.IPAddresses;

            query = query.Where(a => a.Ip == ip);

            return await query.FirstOrDefaultAsync();
        }

        public void Add<T>(T entity) where T : class
        {
            this.logger.LogInformation($"Adding an object of type {entity.GetType()} to the context");
            this.context.Add(entity);
        }

        public async Task<BatchUpdateJob> GetBatchUpdateJobAsync(Guid guid)
        {
            this.logger.LogInformation($"Getting batch update job with guid {guid} from database.");

            IQueryable<BatchUpdateJob> query = this.context.BatchUpdateJobs;

            query = query.Include(j => j.JobItems);

            query = query.Where(a => a.Id == guid);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            this.logger.LogInformation("Trying to save changes in the context");

            try
            {
                //Return true if at least one row has changed
                return (await this.context.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException exc)
            {
                this.logger.LogError(exc, $"{nameof(SaveChangesAsync)} db update error: {exc?.InnerException?.Message}");
                return false;
            }          
        }

        public Task<BatchUpdateJobItem[]> GetUncompleteBatchUpdateJobItemsSortedByDateAsync()
        {
            IQueryable<BatchUpdateJobItem> query = this.context.BatchUpdateJobItems;

            //Include jobs
            query = query.Include(i => i.UpdateJob);

            //Order by date
            query = query.OrderBy(i => i.UpdateJob.DateCreated);

            //Filter
            query = query.Where(i => i.UpdateJob.Complete == false 
                && i.UpdateComplete == false
                && i.Processing == false);

            return query.ToArrayAsync();

        }
    }
}
