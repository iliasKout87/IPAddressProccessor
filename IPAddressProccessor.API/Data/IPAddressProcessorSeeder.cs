using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data
{
    public class IPAddressProcessorSeeder
    {
        private readonly IPAddressesContext context;

        public IPAddressProcessorSeeder(IPAddressesContext context)
        {
            this.context = context;
        }

        public async Task SeedAsync()
        {
            this.context.Database.EnsureCreated();
        }   
    }
}
