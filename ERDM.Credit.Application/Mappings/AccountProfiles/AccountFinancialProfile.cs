using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountFinancialProfile : Profile
    {
        public AccountFinancialProfile()
        {
            // Update Balance DTO to Account
            CreateMap<UpdateBalanceDto, Account>()
                .ForMember(dest => dest.OutstandingBalance, opt => opt.MapFrom(src => src.NewOutstandingBalance))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => new AccountMetadata
                {
                    ApprovalReference = src.ApprovalReference
                }));

            // Adjust Credit DTO to Account
            CreateMap<AdjustCreditDto, Account>()
                .ForMember(dest => dest.AvailableCredit, opt => opt.MapFrom(src => src.NewAvailableCredit))
                .ForMember(dest => dest.ApprovedAmount, opt => opt.MapFrom(src => src.NewCreditLimit))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.AdjustedBy))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => new AccountMetadata
                {
                    ApprovalReference = src.ApprovalReference
                }));

            // Apply Late Fee DTO to Account
            CreateMap<ApplyLateFeeDto, Account>()
                .ForMember(dest => dest.OutstandingBalance, opt => opt.MapFrom((src, dest) => src.AddToBalance ? dest.OutstandingBalance + src.LateFeeAmount : dest.OutstandingBalance))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.AppliedBy));

            // Late Fee to Payment History
            CreateMap<ApplyLateFeeDto, PaymentHistory>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.FeesPaid, opt => opt.MapFrom(src => src.LateFeeAmount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.FeeAppliedDate))
                .ForMember(dest => dest.LateDays, opt => opt.MapFrom(src => src.DaysOverdue))
                .ForMember(dest => dest.LateFeeCharged, opt => opt.MapFrom(src => src.LateFeeAmount))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.LateFeeApplied))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => RepaymentMethod.System))
                .ForMember(dest => dest.TransactionReference, opt => opt.MapFrom(src => $"LATE-{Guid.NewGuid().ToString().Substring(0, 8)}"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.AppliedBy));
        }
    }
}
