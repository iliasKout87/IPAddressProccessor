using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Models
{
    public class CreateBatchUpdateJobRequest
    {
        public IEnumerable<string> IpsToUpdate { get; set; }
    }
}
