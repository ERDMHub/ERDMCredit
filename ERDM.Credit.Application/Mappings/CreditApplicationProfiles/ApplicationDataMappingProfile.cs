using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.CreditApplicationProfiles
{
    public class ApplicationDataMappingProfile : Profile
    {
        public ApplicationDataMappingProfile()
        {
            CreateMap<ApplicationData, ApplicationDataDto>().ReverseMap();
        }
    }
}
