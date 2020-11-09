using IPAddressProccessor.Library.Models.Abstract;
using System.Threading.Tasks;

namespace IPAddressProccessor.Library
{
    public class IPStackIPInfoProvider : IIPInfoProvider
    {
        public async Task<IIPDetails> GetDetails(string ip)
        {
            var ipDetailsClient = new IPStackClient();

            return await ipDetailsClient.Get(ip);
        }
    }
}
