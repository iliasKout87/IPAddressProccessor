using AutoMapper;
using IPAddressProccessor.API.Models;
using IPAddressProccessor.API.Services.Abstract;
using IPAddressProccessor.Library;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Services.Concrete
{
    public class ExternalAPIIPDetailsService : IIPDetailsService
    {
        private readonly IIPInfoProvider provider;
        private readonly ILogger<ExternalAPIIPDetailsService> logger;
        private readonly IMapper mapper;

        public ExternalAPIIPDetailsService(
            IIPInfoProvider provider,
            ILogger<ExternalAPIIPDetailsService> logger,
            IMapper mapper)
        {
            this.provider = provider;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<IPAddressModel> Get(string ip)
        {
            this.logger.LogInformation($"Accessing external API to get details for address: {ip}");

            var result = await this.provider.GetDetails(ip);
            var resultModel = this.mapper.Map<IPAddressModel>(result);
            resultModel.Ip = ip;

            return resultModel;
        }
    }
}
