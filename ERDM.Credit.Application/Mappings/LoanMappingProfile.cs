using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class LoanMappingProfile : Profile
    {
        public LoanMappingProfile()
        {
            // Loan mappings
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.LoanType, opt => opt.MapFrom(src => src.LoanType))
                .ForMember(dest => dest.OriginalAmount, opt => opt.MapFrom(src => src.OriginalAmount))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance))
                .ForMember(dest => dest.MonthlyPayment, opt => opt.MapFrom(src => src.MonthlyPayment))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Secured, opt => opt.MapFrom(src => src.Secured))
                .ForMember(dest => dest.Collateral, opt => opt.MapFrom(src => src.Collateral));

            CreateMap<LoanDto, Loan>()
                .ForMember(dest => dest.LoanType, opt => opt.MapFrom(src => src.LoanType))
                .ForMember(dest => dest.OriginalAmount, opt => opt.MapFrom(src => src.OriginalAmount))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance))
                .ForMember(dest => dest.MonthlyPayment, opt => opt.MapFrom(src => src.MonthlyPayment))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Secured, opt => opt.MapFrom(src => src.Secured))
                .ForMember(dest => dest.Collateral, opt => opt.MapFrom(src => src.Collateral));
        }
    }
}
