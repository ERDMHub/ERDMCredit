using AutoMapper;
using ERDM.Credit.Contracts.DTOs.RiskScoreDtos;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Application.Mappings.RiskScoreProfiles
{
    public class RiskScoreProfile : Profile
    {
        public RiskScoreProfile()
        {
            // Entity to DTO
            CreateMap<RiskScore, RiskScoreResponseDto>()
                .ForMember(dest => dest.ScoreType, opt => opt.MapFrom(src => src.ScoreType.ToString()))
                .ForMember(dest => dest.RiskCategory, opt => opt.MapFrom(src => src.RiskCategory.ToString()))
                .ForMember(dest => dest.RiskFactors, opt => opt.MapFrom(src => src.RiskFactors))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            CreateMap<RiskFactor, RiskFactorDto>();
            CreateMap<RiskScoreMetadata, RiskScoreMetadataDto>();

            // DTO to Entity
            CreateMap<CreateRiskScoreDto, RiskScore>()
                .ForMember(dest => dest.ScoreType, opt => opt.MapFrom(src => Enum.Parse<ScoreType>(src.ScoreType)))
                .ForMember(dest => dest.RiskCategory, opt => opt.MapFrom(src => Enum.Parse<RiskCategory>(src.RiskCategory)))
                .ForMember(dest => dest.RiskFactors, opt => opt.MapFrom(src => src.RiskFactors))
                .ForMember(dest => dest.RiskScoreId, opt => opt.Ignore())
                .ForMember(dest => dest.ScoringDate, opt => opt.Ignore())
                .ForMember(dest => dest.PreviousScore, opt => opt.Ignore())
                .ForMember(dest => dest.ScoreChange, opt => opt.Ignore())
                .ForMember(dest => dest.ProbabilityOfDefault, opt => opt.Ignore())
                .ForMember(dest => dest.LossGivenDefault, opt => opt.Ignore())
                .ForMember(dest => dest.ExposureAtDefault, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedLoss, opt => opt.Ignore())
                .ForMember(dest => dest.IsValid, opt => opt.Ignore())
                .ForMember(dest => dest.ValidUntil, opt => opt.Ignore())
                .ForMember(dest => dest.NextReviewDate, opt => opt.Ignore());

            CreateMap<CreateRiskFactorDto, RiskFactor>()
                .ForMember(dest => dest.FactorId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.AssessedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateRiskScoreDto, RiskScore>()
                .ForMember(dest => dest.RiskCategory, opt => opt.MapFrom(src => Enum.Parse<RiskCategory>(src.RiskCategory)));

            CreateMap<AddRiskFactorDto, RiskFactor>()
                .ForMember(dest => dest.FactorId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.AssessedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}