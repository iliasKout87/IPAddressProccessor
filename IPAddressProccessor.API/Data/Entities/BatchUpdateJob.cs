using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data.Entities
{
    public class BatchUpdateJob
    {
        public Guid Id { get; set; }

        public bool Complete { get; set; }

        public DateTime DateCreated { get; set; }

        public ICollection<BatchUpdateJobItem> JobItems { get; set; }
    }
}
