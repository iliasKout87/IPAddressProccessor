using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data.Entities
{
    public class BatchUpdateJobItem
    {
        public Guid Id { get; set; }

        public string IpToUpdate { get; set; }

        public bool UpdateComplete { get; set; }

        public bool Processing { get; set; }

        public BatchUpdateJob UpdateJob { get; set; }
    }
}
