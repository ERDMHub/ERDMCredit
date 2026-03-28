using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings
{
    public class ApplicationMetadataMappingProfile : Profile
    {
        public ApplicationMetadataMappingProfile()
        {
            CreateMap<ApplicationMetadata, ApplicationMetadataDto>().ReverseMap();
        }
    }
}
