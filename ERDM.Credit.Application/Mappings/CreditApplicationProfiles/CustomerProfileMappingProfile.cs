using AutoMapper;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.CreditApplicationProfiles
{
    public class CustomerProfileMappingProfile : Profile
    {
        public CustomerProfileMappingProfile()
        {
            CreateMap<CustomerProfile, CustomerProfileDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.ResidentialAddress, opt => opt.MapFrom(src => src.ResidentialAddress))
                .ForMember(dest => dest.Employment, opt => opt.MapFrom(src => src.Employment))
                .ForMember(dest => dest.FinancialProfile, opt => opt.MapFrom(src => src.FinancialProfile))
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.MaritalStatus))
                .ForMember(dest => dest.Dependents, opt => opt.MapFrom(src => src.Dependents))
                .ForMember(dest => dest.EducationLevel, opt => opt.MapFrom(src => src.EducationLevel))
                .ForMember(dest => dest.Citizenship, opt => opt.MapFrom(src => src.Citizenship))
                .ForMember(dest => dest.IdVerificationStatus, opt => opt.MapFrom(src => src.IdVerificationStatus))
                .ForMember(dest => dest.IdVerificationDate, opt => opt.MapFrom(src => src.IdVerificationDate))
                .ForMember(dest => dest.IdVerifiedBy, opt => opt.MapFrom(src => src.IdVerifiedBy));

            CreateMap<CustomerProfileDto, CustomerProfile>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.ResidentialAddress, opt => opt.MapFrom(src => src.ResidentialAddress))
                .ForMember(dest => dest.Employment, opt => opt.MapFrom(src => src.Employment))
                .ForMember(dest => dest.FinancialProfile, opt => opt.MapFrom(src => src.FinancialProfile))
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.MaritalStatus))
                .ForMember(dest => dest.Dependents, opt => opt.MapFrom(src => src.Dependents))
                .ForMember(dest => dest.EducationLevel, opt => opt.MapFrom(src => src.EducationLevel))
                .ForMember(dest => dest.Citizenship, opt => opt.MapFrom(src => src.Citizenship))
                .ForMember(dest => dest.IdVerificationStatus, opt => opt.MapFrom(src => src.IdVerificationStatus))
                .ForMember(dest => dest.IdVerificationDate, opt => opt.MapFrom(src => src.IdVerificationDate))
                .ForMember(dest => dest.IdVerifiedBy, opt => opt.MapFrom(src => src.IdVerifiedBy));
        }
    }
}
