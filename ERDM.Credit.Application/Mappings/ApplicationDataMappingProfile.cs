using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class ApplicationDataMappingProfile : Profile
    {
        public ApplicationDataMappingProfile()
        {
            CreateMap<ApplicationData, ApplicationDataDto>().ReverseMap();
        }
    }
}
