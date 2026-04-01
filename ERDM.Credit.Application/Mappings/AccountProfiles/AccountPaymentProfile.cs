using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountPaymentProfile : Profile
    {
        public AccountPaymentProfile()
        {
            // Payment Response DTO
            CreateMap<PaymentHistory, PaymentResponseDto>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.PrincipalPaid, opt => opt.MapFrom(src => src.PrincipalPaid))
                .ForMember(dest => dest.InterestPaid, opt => opt.MapFrom(src => src.InterestPaid))
                .ForMember(dest => dest.FeesPaid, opt => opt.MapFrom(src => src.FeesPaid))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.TransactionReference, opt => opt.MapFrom(src => src.TransactionReference))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.IsLatePayment, opt => opt.MapFrom(src => src.LateDays > 0))
                .ForMember(dest => dest.LateDays, opt => opt.MapFrom(src => src.LateDays))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Payment Installment DTO
            CreateMap<PaymentHistory, PaymentInstallmentDto>()
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.LateDays, opt => opt.MapFrom(src => src.LateDays))
                .ForMember(dest => dest.LateFeeCharged, opt => opt.MapFrom(src => src.LateFeeCharged));

            // Process Payment DTO to PaymentHistory
            CreateMap<ProcessPaymentDto, PaymentHistory>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => Enum.Parse<RepaymentMethod>(src.PaymentMethod)))
                .ForMember(dest => dest.TransactionReference, opt => opt.MapFrom(src => src.TransactionReference))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "system"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Paid))
                .ForMember(dest => dest.AmountDue, opt => opt.Ignore())
                .ForMember(dest => dest.PrincipalPaid, opt => opt.Ignore())
                .ForMember(dest => dest.InterestPaid, opt => opt.Ignore())
                .ForMember(dest => dest.FeesPaid, opt => opt.Ignore())
                .ForMember(dest => dest.DueDate, opt => opt.Ignore())
                .ForMember(dest => dest.LateDays, opt => opt.Ignore())
                .ForMember(dest => dest.LateFeeCharged, opt => opt.Ignore());

            // Reverse Payment DTO to PaymentHistory
            CreateMap<ReversePaymentDto, PaymentHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Reversed))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => $"{src.Comments} - Reversed by: {src.ReversedBy} on {src.ReversalDate} - Reason: {src.ReversalReason}"));
        }
    }
}
