using AutoMapper;
using ERDM.Credit.Contracts.DTOs.CreditDecisionDtos;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Application.Mappings.CreditDecisionProfiles
{
    public class CreditDecisionProfile : Profile
    {
        public CreditDecisionProfile()
        {
            // Entity to DTO
            CreateMap<CreditDecision, CreditDecisionResponseDto>()
                .ForMember(dest => dest.DecisionType, opt => opt.MapFrom(src => src.DecisionType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(src => src.Conditions))
                .ForMember(dest => dest.ApprovalSteps, opt => opt.MapFrom(src => src.ApprovalSteps))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            CreateMap<UnderwritingCondition, UnderwritingConditionDto>();
            CreateMap<ApprovalStep, ApprovalStepDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<DecisionMetadata, DecisionMetadataDto>();

            // DTO to Entity
            CreateMap<CreateCreditDecisionDto, CreditDecision>()
                .ForMember(dest => dest.DecisionType, opt => opt.MapFrom(src => Enum.Parse<DecisionType>(src.DecisionType)))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(src => src.Conditions))
                .ForMember(dest => dest.ApprovalSteps, opt => opt.MapFrom(src => src.ApprovalSteps))
                .ForMember(dest => dest.DecisionId, opt => opt.Ignore())
                .ForMember(dest => dest.DecisionDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<CreateUnderwritingConditionDto, UnderwritingCondition>()
                .ForMember(dest => dest.ConditionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.IsMet, opt => opt.MapFrom(src => false));

            CreateMap<CreateApprovalStepDto, ApprovalStep>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApprovalStepStatus.Pending));

            CreateMap<UpdateUnderwritingConditionDto, UnderwritingCondition>();
            CreateMap<UpdateApprovalStepDto, ApprovalStep>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ApprovalStepStatus>(src.Status)));

            CreateMap<ApproveCreditDecisionDto, CreditDecision>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DecisionStatus.Completed))
                .ForMember(dest => dest.DecisionType, opt => opt.MapFrom(src => DecisionType.Approved))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.ApprovedBy));

            CreateMap<DeclineCreditDecisionDto, CreditDecision>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DecisionStatus.Completed))
                .ForMember(dest => dest.DecisionType, opt => opt.MapFrom(src => DecisionType.Declined))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.DeclinedBy));
        }
    }
}
