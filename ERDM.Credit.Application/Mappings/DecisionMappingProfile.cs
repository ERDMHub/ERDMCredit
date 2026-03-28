using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings
{
    public class DecisionMappingProfile : Profile
    {
        public DecisionMappingProfile()
        {
            CreateMap<Decision, DecisionDto>().ReverseMap();
        }
    }
}
