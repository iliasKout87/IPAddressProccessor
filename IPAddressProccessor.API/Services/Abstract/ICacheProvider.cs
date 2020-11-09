using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Abstract
{
    public interface ICacheProvider
    {
        T GetFromCache<T>(string key) where T : class;

        void SetCache<T>(string key, T value) where T : class;
    }
}
