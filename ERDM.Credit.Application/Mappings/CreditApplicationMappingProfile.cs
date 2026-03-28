using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class CreditApplicationMappingProfile : Profile
    {
        public CreditApplicationMappingProfile()
        {
            // ✅ Entity → Response DTO
            CreateMap<CreditApplication, CreditApplicationResponseDto>();


            // ✅ Request DTO → Entity (CREATE)
            CreateMap<CreditApplicationRequestDto, CreditApplication>()

                // 🔴 Ignore system/domain-controlled fields
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Decision, opt => opt.Ignore())
                .ForMember(dest => dest.CreditBureauData, opt => opt.Ignore())
                .ForMember(dest => dest.FraudCheck, opt => opt.Ignore())
                .ForMember(dest => dest.UnderwritingHistory, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())

                // ✅ Use constructor (DDD pattern)
                .ConstructUsing((src, context) => new CreditApplication(
                    src.CustomerId,
                    context.Mapper.Map<CustomerProfile>(src.CustomerProfile),
                    src.ProductType,
                    src.RequestedAmount,
                    src.RequestedTerm,
                    context.Mapper.Map<ApplicationData>(src.ApplicationData),
                    context.Mapper.Map<ApplicationMetadata>(src.Metadata)
                ));
        }
    }
}