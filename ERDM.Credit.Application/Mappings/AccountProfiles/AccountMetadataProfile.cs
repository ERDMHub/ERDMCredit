using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountMetadataProfile : Profile
    {
        public AccountMetadataProfile()
        {
            // AccountMetadata Entity to DTO
            CreateMap<AccountMetadata, AccountMetadataDto>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.UserAgent, opt => opt.MapFrom(src => src.UserAgent))
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.ApprovalReference, opt => opt.MapFrom(src => src.ApprovalReference))
                .ForMember(dest => dest.DisbursementReference, opt => opt.MapFrom(src => src.DisbursementReference))
                .ForMember(dest => dest.ContractSigned, opt => opt.MapFrom(src => src.ContractSigned))
                .ForMember(dest => dest.ContractSignedDate, opt => opt.MapFrom(src => src.ContractSignedDate))
                .ForMember(dest => dest.DocumentsUploaded, opt => opt.MapFrom(src => src.DocumentsUploaded))
                .ForMember(dest => dest.AutoDebitEnabled, opt => opt.MapFrom(src => src.AutoDebitEnabled))
                .ForMember(dest => dest.AutoDebitAccount, opt => opt.MapFrom(src => src.AutoDebitAccount))
                .ForMember(dest => dest.NotificationsEnabled, opt => opt.MapFrom(src => src.NotificationsEnabled))
                .ForMember(dest => dest.EmailAlerts, opt => opt.MapFrom(src => src.EmailAlerts))
                .ForMember(dest => dest.SmsAlerts, opt => opt.MapFrom(src => src.SmsAlerts));

            // AccountMetadata DTO to Entity
            CreateMap<AccountMetadataDto, AccountMetadata>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.UserAgent, opt => opt.MapFrom(src => src.UserAgent))
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.ApprovalReference, opt => opt.MapFrom(src => src.ApprovalReference))
                .ForMember(dest => dest.DisbursementReference, opt => opt.MapFrom(src => src.DisbursementReference))
                .ForMember(dest => dest.ContractSigned, opt => opt.MapFrom(src => src.ContractSigned))
                .ForMember(dest => dest.ContractSignedDate, opt => opt.MapFrom(src => src.ContractSignedDate))
                .ForMember(dest => dest.DocumentsUploaded, opt => opt.MapFrom(src => src.DocumentsUploaded))
                .ForMember(dest => dest.AutoDebitEnabled, opt => opt.MapFrom(src => src.AutoDebitEnabled))
                .ForMember(dest => dest.AutoDebitAccount, opt => opt.MapFrom(src => src.AutoDebitAccount))
                .ForMember(dest => dest.NotificationsEnabled, opt => opt.MapFrom(src => src.NotificationsEnabled))
                .ForMember(dest => dest.EmailAlerts, opt => opt.MapFrom(src => src.EmailAlerts))
                .ForMember(dest => dest.SmsAlerts, opt => opt.MapFrom(src => src.SmsAlerts));
        }
    }
}
