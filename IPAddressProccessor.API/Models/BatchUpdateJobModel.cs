using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Models
{
    public class BatchUpdateJobModel
    {
        public Guid Id { get; set; }

        public bool Complete { get; set; }

        public DateTime DateCreated { get; set; }

        public ICollection<BatchUpdateJobItemModel> JobItems { get; set; }
    }
}
