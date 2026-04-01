using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountStatusHistoryProfile : Profile
    {
        public AccountStatusHistoryProfile()
        {
            // AccountStatusHistory Entity to DTO
            CreateMap<AccountStatusHistory, AccountStatusHistoryDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            // Create status history from various DTOs
            CreateMap<ActivateAccountDto, AccountStatusHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Active))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ActivationDate))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ActivatedBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.ActivationReason))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<CloseAccountDto, AccountStatusHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Closed))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ClosureDate))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ClosedBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.ClosureReason))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<SuspendAccountDto, AccountStatusHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Suspended))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.SuspensionDate))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.SuspendedBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.SuspensionReason))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<MarkDelinquentDto, AccountStatusHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Delinquent))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.MarkedBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.DelinquencyReason ?? "Account became delinquent"))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<WriteOffAccountDto, AccountStatusHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.WrittenOff))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.WrittenOffBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.WriteOffReason))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<RestructureAccountDto, AccountStatusHistory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Restructured))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.RestructuredBy))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.RestructuringReason))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
        }
    }
}
