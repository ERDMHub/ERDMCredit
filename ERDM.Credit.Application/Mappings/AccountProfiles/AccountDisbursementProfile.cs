using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountDisbursementProfile : Profile
    {
        public AccountDisbursementProfile()
        {
            // Disburse Amount DTO to Account
            CreateMap<DisburseAmountDto, Account>()
                .ForMember(dest => dest.DisbursedAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Disbursed))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.DisbursedBy))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => new AccountMetadata
                {
                    DisbursementReference = src.TransactionReference,
                    ApprovalReference = src.ApprovalReference
                }));

            // Confirm Disbursement DTO to Account
            CreateMap<ConfirmDisbursementDto, Account>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.ConfirmedBy))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => new AccountMetadata
                {
                    DisbursementReference = src.TransactionReference
                }));
        }
    }
}
