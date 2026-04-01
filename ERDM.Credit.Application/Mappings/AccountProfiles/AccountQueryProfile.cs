using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountQueryProfile : Profile
    {
        public AccountQueryProfile()
        {
            // Account Query DTO to Account Filter
            CreateMap<AccountQueryDto, AccountFilter>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.AccountStatus != null ? Enum.Parse<AccountStatus>(src.AccountStatus) : (AccountStatus?)null))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType != null ? Enum.Parse<AccountType>(src.AccountType) : (AccountType?)null))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate))
                .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate))
                .ForMember(dest => dest.MinBalance, opt => opt.MapFrom(src => src.MinBalance))
                .ForMember(dest => dest.MaxBalance, opt => opt.MapFrom(src => src.MaxBalance))
                .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode))
                .ForMember(dest => dest.AssignedOfficer, opt => opt.MapFrom(src => src.AssignedOfficer))
                .ForMember(dest => dest.IsDelinquent, opt => opt.MapFrom(src => src.IsDelinquent))
                .ForMember(dest => dest.MinOverdueDays, opt => opt.MapFrom(src => src.MinOverdueDays))
                .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.SearchTerm));

            // Payment History Query DTO
            CreateMap<PaymentHistoryQueryDto, PaymentHistoryFilter>()
                .ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate))
                .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus != null ? Enum.Parse<PaymentStatus>(src.PaymentStatus) : (PaymentStatus?)null))
                .ForMember(dest => dest.IsLatePayment, opt => opt.MapFrom(src => src.IsLatePayment));
        }
    }
}
