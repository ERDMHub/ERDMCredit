using AutoMapper;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.CreditApplicationProfiles
{
    public class EmploymentMappingProfile : Profile
    {
        public EmploymentMappingProfile()
        {
            // ✅ Entity → DTO
            CreateMap<Employment, EmploymentDto>();

            // ✅ DTO → Entity
            CreateMap<EmploymentDto, Employment>()
                .ForMember(dest => dest.MonthsEmployed, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerPhone, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerAddress, opt => opt.Ignore())
                .ForMember(dest => dest.IncomeType, opt => opt.Ignore())
                .ForMember(dest => dest.AdditionalIncome, opt => opt.Ignore())
                .ForMember(dest => dest.IncomeVerified, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedBy, opt => opt.Ignore());
        }
    }
}