using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountReportProfile : Profile
    {
        public AccountReportProfile()
        {
            // Account Summary DTO
            CreateMap<Account, AccountSummaryItemDto>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType.ToString()))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OutstandingBalance, opt => opt.MapFrom(src => src.OutstandingBalance))
                .ForMember(dest => dest.EmiAmount, opt => opt.MapFrom(src => src.EmiAmount))
                .ForMember(dest => dest.NextPaymentDueDate, opt => opt.MapFrom(src => src.NextPaymentDueDate))
                .ForMember(dest => dest.IsDelinquent, opt => opt.MapFrom(src => src.IsDelinquent))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue))
                .ForMember(dest => dest.OpeningDate, opt => opt.MapFrom(src => src.OpeningDate))
                .ForMember(dest => dest.MaturityDate, opt => opt.MapFrom(src => src.MaturityDate));

            // Aging Account DTO
            CreateMap<Account, AgingAccountDto>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OutstandingBalance, opt => opt.MapFrom(src => src.OutstandingBalance))
                .ForMember(dest => dest.LastPaymentDate, opt => opt.MapFrom(src => src.LastPaymentDate))
                .ForMember(dest => dest.NextPaymentDueDate, opt => opt.MapFrom(src => src.NextPaymentDueDate))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue))
                .ForMember(dest => dest.AgingBucket, opt => opt.MapFrom(src => GetAgingBucket(src.DaysOverdue)))
                .ForMember(dest => dest.AssignedOfficer, opt => opt.MapFrom(src => src.AssignedOfficer))
                .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode))
                .ForMember(dest => dest.CollectionAttempts, opt => opt.MapFrom(src => src.PaymentHistory.Count(p => p.Status == PaymentStatus.Overdue)))
                .ForMember(dest => dest.LastCollectionAttempt, opt => opt.MapFrom(src => src.LastCollectionAttempt));

            // Amortization Schedule
            CreateMap<AmortizationScheduleDto, AmortizationScheduleDto>();
            CreateMap<AmortizationEntryDto, AmortizationEntryDto>();
        }

        private static string GetAgingBucket(int daysOverdue)
        {
            if (daysOverdue <= 0) return "Current";
            if (daysOverdue <= 30) return "1-30 Days";
            if (daysOverdue <= 60) return "31-60 Days";
            if (daysOverdue <= 90) return "61-90 Days";
            return "90+ Days";
        }
    }
}
