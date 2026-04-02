using AutoMapper;
using ERDM.Credit.Contracts.DTOs.LimitHistoryDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.LimitHistoryProfiles
{
     public class LimitHistoryProfile : Profile
    {
        public LimitHistoryProfile()
        {
            // Entity to DTO
            CreateMap<LimitHistory, LimitHistoryResponseDto>()
                .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => src.ChangeType.ToString()))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            CreateMap<LimitHistory, LimitHistorySummaryItemDto>()
                .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => src.ChangeType.ToString()));

            CreateMap<LimitHistoryMetadata, LimitHistoryMetadataDto>();

            // DTO to Entity
            CreateMap<CreateLimitHistoryDto, LimitHistory>()
                .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => Enum.Parse<LimitChangeType>(src.ChangeType)))
                .ForMember(dest => dest.LimitHistoryId, opt => opt.Ignore())
                .ForMember(dest => dest.ChangeAmount, opt => opt.Ignore())
                .ForMember(dest => dest.ChangePercentage, opt => opt.Ignore())
                .ForMember(dest => dest.ChangedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiryDate, opt => opt.Ignore())
                .ForMember(dest => dest.Metadata, opt => opt.Ignore());

            CreateMap<UpdateLimitHistoryDto, LimitHistory>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate));
        }
    }
}