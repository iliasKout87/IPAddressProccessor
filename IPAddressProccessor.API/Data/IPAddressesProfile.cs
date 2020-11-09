using AutoMapper;
using IPAddressProccessor.API.Data.Entities;
using IPAddressProccessor.API.Models;
using IPAddressProccessor.Library.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPAddressProccessor.API.Data
{
    public class IPAddressesProfile: Profile
    {
        public IPAddressesProfile()
        {
            this.CreateMap<IPAddress, IPAddressModel>()
                    .ReverseMap();
            this.CreateMap<IIPDetails, IPAddressModel>();
            this.CreateMap<CreateBatchUpdateJobRequest, BatchUpdateJobModel>()
                    .ForMember(d => d.Id, s => s.MapFrom(i => Guid.NewGuid()))
                    .ForMember(d => d.Complete, s => s.MapFrom(i => false))
                    .ForMember(d => d.DateCreated, s => s.MapFrom(i => DateTime.Now))
                    .ForMember(d => d.JobItems, s => s.MapFrom(i => i.IpsToUpdate.Select(ip => new BatchUpdateJobItemModel() { Id = Guid.NewGuid(), IpToUpdate = ip, UpdateComplete = false })));
            this.CreateMap<BatchUpdateJobItem, BatchUpdateJobItemModel>()
                    .ReverseMap();
            this.CreateMap<BatchUpdateJob, BatchUpdateJobModel>()
                    .ReverseMap();
        }
    }
}
