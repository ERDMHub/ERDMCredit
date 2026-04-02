using AutoMapper;
using ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Application.Mappings.UnderwritingRuleProfiles
{
    public class UnderwritingRuleProfile : Profile
    {
        public UnderwritingRuleProfile()
        {
            // Entity to DTO
            CreateMap<UnderwritingRule, UnderwritingRuleResponseDto>()
                .ForMember(dest => dest.RuleType, opt => opt.MapFrom(src => src.RuleType.ToString()))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => src.Actions))
                .ForMember(dest => dest.TrueOutcome, opt => opt.MapFrom(src => src.TrueOutcome))
                .ForMember(dest => dest.FalseOutcome, opt => opt.MapFrom(src => src.FalseOutcome))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            CreateMap<RuleAction, RuleActionDto>();
            CreateMap<RuleOutcome, RuleOutcomeDto>();
            CreateMap<UnderwritingRuleMetadata, UnderwritingRuleMetadataDto>();

            // DTO to Entity
            CreateMap<CreateUnderwritingRuleDto, UnderwritingRule>()
                .ForMember(dest => dest.RuleType, opt => opt.MapFrom(src => Enum.Parse<RuleType>(src.RuleType)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<RuleCategory>(src.Category)))
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => src.Actions))
                .ForMember(dest => dest.TrueOutcome, opt => opt.MapFrom(src => src.TrueOutcome))
                .ForMember(dest => dest.FalseOutcome, opt => opt.MapFrom(src => src.FalseOutcome))
                .ForMember(dest => dest.DependsOnRules, opt => opt.MapFrom(src => src.DependsOnRules ?? new List<string>()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()))
                .ForMember(dest => dest.RuleId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.RuleVersion, opt => opt.Ignore())
                .ForMember(dest => dest.EffectiveFrom, opt => opt.Ignore())
                .ForMember(dest => dest.SuccessRate, opt => opt.Ignore())
                .ForMember(dest => dest.ExecutionCount, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore());

            CreateMap<CreateRuleActionDto, RuleAction>()
                .ForMember(dest => dest.Parameters, opt => opt.MapFrom(src => src.Parameters ?? new Dictionary<string, object>()));

            CreateMap<CreateRuleOutcomeDto, RuleOutcome>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data ?? new Dictionary<string, object>()))
                .ForMember(dest => dest.NextRules, opt => opt.MapFrom(src => src.NextRules ?? new List<string>()));

            CreateMap<UpdateUnderwritingRuleDto, UpdateRuleData>();
            CreateMap<UpdateRuleActionDto, RuleAction>();
            CreateMap<UpdateRuleOutcomeDto, RuleOutcome>();

            // Execution results
            CreateMap<RuleOutcome, RuleOutcomeDto>();
            CreateMap<RuleAction, RuleActionDto>();
        }
    }
}