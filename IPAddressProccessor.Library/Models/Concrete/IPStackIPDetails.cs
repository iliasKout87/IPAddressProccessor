using IPAddressProccessor.Library.Models.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPAddressProccessor.Library.Models.Concrete
{
    public class IPStackIPDetails : IIPDetails
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_name")]
        public string Country { get; set; }

        [JsonProperty("continent_name")]
        public string Continent { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
