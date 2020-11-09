using IPAddressProccessor.Library.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IPAddressProccessor.Library
{
    public interface IIPInfoProvider
    {
        Task<IIPDetails> GetDetails(string ip);
    }
}
