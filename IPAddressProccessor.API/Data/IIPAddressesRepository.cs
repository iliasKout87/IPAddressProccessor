using IPAddressProccessor.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data
{
    public interface IIPAddressesRepository
    {
        Task<IPAddress> GetIPAddressDetailsAsync(string ip);

        Task<BatchUpdateJob> GetBatchUpdateJobAsync(Guid guid);

        void Add<T>(T entity) where T : class;

        Task<bool> SaveChangesAsync();

        Task<BatchUpdateJobItem[]> GetUncompleteBatchUpdateJobItemsSortedByDateAsync();
    }
}
