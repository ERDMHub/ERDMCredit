using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            // Entity → DTO
            CreateMap<Address, AddressDto>();

            // DTO → Entity
            CreateMap<AddressDto, Address>()
                .ForMember(dest => dest.AddressType, opt => opt.Ignore())
                .ForMember(dest => dest.IsPrimary, opt => opt.Ignore())
                .ForMember(dest => dest.YearsAtAddress, opt => opt.Ignore())
                .ForMember(dest => dest.MonthsAtAddress, opt => opt.Ignore());
        }
    }
}