using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Models
{
    public class BatchUpdateJobProgressResponse
    {
        public int UpdatesComplete { get; set; }

        public int UpdatesTotal { get; set; }
    }
}
