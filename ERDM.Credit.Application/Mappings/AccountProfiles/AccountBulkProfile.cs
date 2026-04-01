using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountBulkProfile : Profile
    {
        public AccountBulkProfile()
        {
            // Bulk Status Update
            CreateMap<BulkStatusUpdateDto, Account>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<AccountStatus>(src.NewStatus)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => new AccountMetadata
                {
                    ApprovalReference = src.ApprovalReference
                }));

            // Bulk Overdue DTO to Account
            CreateMap<BulkOverdueDto, Account>()
                .ForMember(dest => dest.IsDelinquent, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Delinquent))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.MarkedBy))
                .ForMember(dest => dest.LastCollectionAttempt, opt => opt.MapFrom(src => src.AsOfDate));
        }
    }
}
