using AutoMapper;
using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings
{
    public class CreditCardMappingProfile : Profile
    {
        public CreditCardMappingProfile()
        {
            // CreditCard mappings
            CreateMap<CreditCard, CreditCardDto>()
                    .ForMember(dest => dest.Issuer, opt => opt.MapFrom(src => src.Issuer))
                    .ForMember(dest => dest.CreditLimit, opt => opt.MapFrom(src => src.CreditLimit))
                    .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance))
                    .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.PaymentAmount))
                    .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
                    .ForMember(dest => dest.OpenedDate, opt => opt.MapFrom(src => src.OpenedDate));

            CreateMap<CreditCardDto, CreditCard>()
                .ForMember(dest => dest.Issuer, opt => opt.MapFrom(src => src.Issuer))
                .ForMember(dest => dest.CreditLimit, opt => opt.MapFrom(src => src.CreditLimit))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance))
                .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.PaymentAmount))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
                .ForMember(dest => dest.OpenedDate, opt => opt.MapFrom(src => src.OpenedDate));
        }
    }
}
