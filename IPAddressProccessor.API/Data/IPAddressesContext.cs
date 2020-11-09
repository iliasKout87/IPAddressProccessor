using IPAddressProccessor.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data
{
    public class IPAddressesContext: DbContext
    {
        public IPAddressesContext(DbContextOptions<IPAddressesContext> options)
            :base(options)
        {

        }

        public DbSet<IPAddress> IPAddresses { get; set; }

        public DbSet<BatchUpdateJob> BatchUpdateJobs { get; set; }

        public DbSet<BatchUpdateJobItem> BatchUpdateJobItems { get; set; }
    }
}
