using IPAddressProccessor.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Abstract
{
    public interface IIPDetailsService
    {
        Task<IPAddressModel> Get(string ip);
    }
}
