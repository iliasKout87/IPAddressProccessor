using IPAddressProccessor.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Abstract
{
    public interface IBatchUpdateJobsService
    {
        Task<BatchUpdateJobModel> Create(CreateBatchUpdateJobRequest request);

        Task<BatchUpdateJobModel> Get(Guid jobId);

        Task<bool> DoUpdate();
    }
}
