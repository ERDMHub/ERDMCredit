using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Entity to Response DTO
            CreateMap<Account, AccountResponseDto>()
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType.ToString()))
                .ForMember(dest => dest.InterestType, opt => opt.MapFrom(src => src.InterestType.ToString()))
                .ForMember(dest => dest.RepaymentFrequency, opt => opt.MapFrom(src => src.RepaymentFrequency.ToString()))
                .ForMember(dest => dest.RepaymentMethod, opt => opt.MapFrom(src => src.RepaymentMethod.ToString()))
                .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => src.Channel.ToString()))
                .ForMember(dest => dest.CollateralDetails, opt => opt.MapFrom(src => src.CollateralDetails))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            // Create DTO to Entity
            CreateMap<CreateAccountDto, Account>()
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => Enum.Parse<AccountType>(src.AccountType)))
                .ForMember(dest => dest.InterestType, opt => opt.MapFrom(src => Enum.Parse<InterestType>(src.InterestType)))
                .ForMember(dest => dest.RepaymentFrequency, opt => opt.MapFrom(src => Enum.Parse<RepaymentFrequency>(src.RepaymentFrequency)))
                .ForMember(dest => dest.RepaymentMethod, opt => opt.MapFrom(src => Enum.Parse<RepaymentMethod>(src.RepaymentMethod)))
                .ForMember(dest => dest.CollateralDetails, opt => opt.MapFrom(src => src.CollateralDetails))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.AccountNumber, opt => opt.Ignore())
                .ForMember(dest => dest.OutstandingBalance, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableCredit, opt => opt.Ignore())
                .ForMember(dest => dest.EmiAmount, opt => opt.Ignore())
                .ForMember(dest => dest.StatusHistory, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentHistory, opt => opt.Ignore())
                .ForMember(dest => dest.Metadata, opt => opt.Ignore());

            // Create From Application DTO to Entity
            CreateMap<CreateAccountFromApplicationDto, Account>()
                .ForMember(dest => dest.RepaymentMethod, opt => opt.MapFrom(src => Enum.Parse<RepaymentMethod>(src.RepaymentMethod)));
        }

        private static string GenerateAccountId() => Guid.NewGuid().ToString();
        private static string GenerateAccountNumber() => $"ACC-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        private static decimal CalculateEMI(decimal principal, decimal rate, int months, string interestType) => principal * (rate / 1200) * (decimal)Math.Pow(1 + (double)(rate / 1200), months) / (decimal)(Math.Pow(1 + (double)(rate / 1200), months) - 1);
        private static decimal CalculateProcessingFee(decimal amount) => amount * 0.01m;
        private static decimal CalculateLatePaymentFee(decimal amount) => amount * 0.02m;
    }
}
