using IPAddressProccessor.API.Models.Validation_Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Models
{
    public class GetIPAddressDetailsRequest
    {
        [Required]
        [IPv4Validator(ErrorMessage ="The provided address is not a valid IPv4 address")]
        public string Ip { get; set; }
    }
}
