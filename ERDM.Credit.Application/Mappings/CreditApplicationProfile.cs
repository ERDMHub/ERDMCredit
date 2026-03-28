using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class CreditApplicationProfile : Profile
    {
        public CreditApplicationProfile()
        {
            // Entity to DTO
            CreateMap<CreditApplication, CreditApplicationResponseDto>();
            CreateMap<CustomerProfile, CustomerProfileDto>();
            CreateMap<ApplicationData, ApplicationDataDto>();
            CreateMap<Decision, DecisionDto>();

            // DTO to Entity
            CreateMap<CreateCreditApplicationDto, CreditApplication>()
                .ConstructUsing((src, ctx) => new CreditApplication(
                    src.CustomerId,
                    ctx.Mapper.Map<CustomerProfile>(src.CustomerProfile),
                    src.ProductType,
                    src.RequestedAmount,
                    src.RequestedTerm,
                    ctx.Mapper.Map<ApplicationData>(src.ApplicationData),
                    ctx.Mapper.Map<ApplicationMetadata>(src.Metadata)));
        }
    }
}
