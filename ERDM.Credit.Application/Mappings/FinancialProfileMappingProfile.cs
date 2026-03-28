using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class FinancialProfileMappingProfile : Profile
    {
        public FinancialProfileMappingProfile()
        {
            CreateMap<FinancialProfile, FinancialProfileDto>()
                .ForMember(dest => dest.MonthlyExpenses, opt => opt.MapFrom(src => src.MonthlyExpenses))
                .ForMember(dest => dest.ExistingDebt, opt => opt.MapFrom(src => src.ExistingDebt))
                .ForMember(dest => dest.CreditScore, opt => opt.MapFrom(src => src.CreditScore))
                .ForMember(dest => dest.SavingsAmount, opt => opt.MapFrom(src => src.SavingsAmount))
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.OtherAssets, opt => opt.MapFrom(src => src.OtherAssets))
                .ForMember(dest => dest.Liabilities, opt => opt.MapFrom(src => src.Liabilities))
                .ForMember(dest => dest.MonthlyDebtPayments, opt => opt.MapFrom(src => src.MonthlyDebtPayments))
                .ForMember(dest => dest.CreditCards, opt => opt.MapFrom(src => src.CreditCards))
                .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans))
                .ForMember(dest => dest.BankStatementsProvided, opt => opt.MapFrom(src => src.BankStatementsProvided))
                .ForMember(dest => dest.LastCreditCheckDate, opt => opt.MapFrom(src => src.LastCreditCheckDate));

            CreateMap<FinancialProfileDto, FinancialProfile>()
                .ForMember(dest => dest.MonthlyExpenses, opt => opt.MapFrom(src => src.MonthlyExpenses))
                .ForMember(dest => dest.ExistingDebt, opt => opt.MapFrom(src => src.ExistingDebt))
                .ForMember(dest => dest.CreditScore, opt => opt.MapFrom(src => src.CreditScore))
                .ForMember(dest => dest.SavingsAmount, opt => opt.MapFrom(src => src.SavingsAmount))
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.OtherAssets, opt => opt.MapFrom(src => src.OtherAssets))
                .ForMember(dest => dest.Liabilities, opt => opt.MapFrom(src => src.Liabilities))
                .ForMember(dest => dest.MonthlyDebtPayments, opt => opt.MapFrom(src => src.MonthlyDebtPayments))
                .ForMember(dest => dest.CreditCards, opt => opt.MapFrom(src => src.CreditCards))
                .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans))
                .ForMember(dest => dest.BankStatementsProvided, opt => opt.MapFrom(src => src.BankStatementsProvided))
                .ForMember(dest => dest.LastCreditCheckDate, opt => opt.MapFrom(src => src.LastCreditCheckDate));
        }
    }
}
