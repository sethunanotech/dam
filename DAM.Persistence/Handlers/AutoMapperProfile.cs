using AutoMapper;
using DAM.Application.DTO.Requests;
using DAM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Persistence.Handlers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<OrganizationRequest,Organization>();
        }
    }
}
