using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPAddressProccessor.API.Models;
using IPAddressProccessor.API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPAddressProccessor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPDetailsController : ControllerBase
    {
        private readonly IIPDetailsService ipDetailsService;

        public IPDetailsController(IIPDetailsService ipDetailsService)
        {
            this.ipDetailsService = ipDetailsService;
        }

        [HttpPost]
        public async Task<ActionResult<IPAddressModel>> Get(GetIPAddressDetailsRequest request)
        {
            try
            {
                var result = await this.ipDetailsService.Get(request.Ip);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error getting details for the ip: {request.Ip}. Inner exception: {ex}");
            }
        }

        
    }
}