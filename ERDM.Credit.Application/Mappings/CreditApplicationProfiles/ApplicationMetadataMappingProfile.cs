using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.CreditApplicationProfiles
{
    public class ApplicationMetadataMappingProfile : Profile
    {
        public ApplicationMetadataMappingProfile()
        {
            CreateMap<ApplicationMetadata, ApplicationMetadataDto>().ReverseMap();
        }
    }
}
