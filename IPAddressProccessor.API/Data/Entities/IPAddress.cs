﻿using System;

namespace IPAddressProccessor.API.Data.Entities
{
    public class IPAddress
    {
        public Guid Id { get; set; }
        public string Ip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
