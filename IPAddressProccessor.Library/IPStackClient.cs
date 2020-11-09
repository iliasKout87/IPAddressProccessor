using IPAddressProccessor.Library.Exceptions;
using IPAddressProccessor.Library.Models.Concrete;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IPAddressProccessor.Library
{
    public class IPStackClient
    {
        private readonly string baseUrl = "http://api.ipstack.com/";
        private readonly string accessKey = "4a007214e596e6050aa2af11b0a406fb";

        public async Task<IPStackIPDetails> Get(string ip)
        {
            var request = new RestRequest();
            request.AddParameter("IpAddress", ip, ParameterType.UrlSegment);
            request.Resource = "{IpAddress}";

            return await this.ExecuteRequest<IPStackIPDetails>(request);
        }

        private async Task<T> ExecuteRequest<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl);
            client.ThrowOnAnyError = true;

            request.AddParameter("access_key", this.accessKey, ParameterType.QueryString);
            
            try
            {
                var response = await client.ExecuteAsync(request);

                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch(Exception ex)
            {
                throw new IPServiceNotAvailableException("Request to service failed.", ex);
            }
        }
    }
}
