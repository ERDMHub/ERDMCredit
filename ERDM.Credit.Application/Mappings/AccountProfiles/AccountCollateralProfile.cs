using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountCollateralProfile : Profile
    {
        public AccountCollateralProfile()
        {
            // CollateralDetails Entity to DTO
            CreateMap<CollateralDetails, CollateralDetailsDto>()
                .ForMember(dest => dest.CollateralType, opt => opt.MapFrom(src => src.CollateralType))
                .ForMember(dest => dest.CollateralValue, opt => opt.MapFrom(src => src.CollateralValue))
                .ForMember(dest => dest.CollateralDescription, opt => opt.MapFrom(src => src.CollateralDescription))
                .ForMember(dest => dest.CollateralDocuments, opt => opt.MapFrom(src => src.CollateralDocuments))
                .ForMember(dest => dest.ValuationDate, opt => opt.MapFrom(src => src.ValuationDate))
                .ForMember(dest => dest.ValuationOfficer, opt => opt.MapFrom(src => src.ValuationOfficer))
                .ForMember(dest => dest.InsuranceDetails, opt => opt.MapFrom(src => src.InsuranceDetails));

            // CollateralDetails DTO to Entity
            CreateMap<CollateralDetailsDto, CollateralDetails>()
                .ForMember(dest => dest.CollateralType, opt => opt.MapFrom(src => src.CollateralType))
                .ForMember(dest => dest.CollateralValue, opt => opt.MapFrom(src => src.CollateralValue))
                .ForMember(dest => dest.CollateralDescription, opt => opt.MapFrom(src => src.CollateralDescription))
                .ForMember(dest => dest.CollateralDocuments, opt => opt.MapFrom(src => src.CollateralDocuments))
                .ForMember(dest => dest.ValuationDate, opt => opt.MapFrom(src => src.ValuationDate))
                .ForMember(dest => dest.ValuationOfficer, opt => opt.MapFrom(src => src.ValuationOfficer))
                .ForMember(dest => dest.InsuranceDetails, opt => opt.MapFrom(src => src.InsuranceDetails));

            // InsuranceDetails Entity to DTO
            CreateMap<InsuranceDetails, InsuranceDetailsDto>()
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumber))
                .ForMember(dest => dest.InsuranceProvider, opt => opt.MapFrom(src => src.InsuranceProvider))
                .ForMember(dest => dest.CoverageAmount, opt => opt.MapFrom(src => src.CoverageAmount))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
                .ForMember(dest => dest.PremiumPaid, opt => opt.MapFrom(src => src.PremiumPaid));

            // InsuranceDetails DTO to Entity
            CreateMap<InsuranceDetailsDto, InsuranceDetails>()
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumber))
                .ForMember(dest => dest.InsuranceProvider, opt => opt.MapFrom(src => src.InsuranceProvider))
                .ForMember(dest => dest.CoverageAmount, opt => opt.MapFrom(src => src.CoverageAmount))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
                .ForMember(dest => dest.PremiumPaid, opt => opt.MapFrom(src => src.PremiumPaid));
        }
    }
}
