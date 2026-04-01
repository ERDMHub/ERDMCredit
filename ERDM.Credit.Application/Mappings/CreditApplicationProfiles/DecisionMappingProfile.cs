using AutoMapper;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.CreditApplicationProfiles
{
    public class DecisionMappingProfile : Profile
    {
        public DecisionMappingProfile()
        {
            CreateMap<Decision, DecisionDto>().ReverseMap();
        }
    }
}
