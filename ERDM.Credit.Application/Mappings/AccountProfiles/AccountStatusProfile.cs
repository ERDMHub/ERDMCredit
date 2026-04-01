using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Application.Mappings.AccountProfiles
{
    public class AccountStatusProfile : Profile
    {
        public AccountStatusProfile()
        {
            // Activate Account
            CreateMap<ActivateAccountDto, Account>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Active))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.ActivatedBy))
                .ForMember(dest => dest.StatusHistory, opt => opt.Ignore());

            // Close Account
            CreateMap<CloseAccountDto, Account>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Closed))
                .ForMember(dest => dest.ClosingDate, opt => opt.MapFrom(src => src.ClosureDate))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.ClosedBy))
                .ForMember(dest => dest.StatusHistory, opt => opt.Ignore());

            // Suspend Account
            CreateMap<SuspendAccountDto, Account>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Suspended))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.SuspendedBy))
                .ForMember(dest => dest.StatusHistory, opt => opt.Ignore());

            // Mark as Delinquent
            CreateMap<MarkDelinquentDto, Account>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Delinquent))
                .ForMember(dest => dest.IsDelinquent, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue))
                .ForMember(dest => dest.LastCollectionAttempt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CollectionOfficer, opt => opt.MapFrom(src => src.AssignedCollectionOfficer))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.MarkedBy));

            // Write Off Account
            CreateMap<WriteOffAccountDto, Account>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.WrittenOff))
                .ForMember(dest => dest.ClosingDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.WrittenOffBy));

            // Restructure Account
            CreateMap<RestructureAccountDto, Account>()
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Restructured))
                  .ForMember(dest => dest.TermMonths, opt => opt.MapFrom((src, dest) => src.NewTermMonths ?? dest.TermMonths))
                  .ForMember(dest => dest.TermYears, opt => opt.MapFrom((src, dest) =>
                      src.NewTermMonths.HasValue ? src.NewTermMonths.Value / 12 : dest.TermYears))
                  .ForMember(dest => dest.InterestRate, opt => opt.MapFrom((src, dest) => src.NewInterestRate ?? dest.InterestRate))
                  .ForMember(dest => dest.EmiAmount, opt => opt.MapFrom((src, dest) => src.NewEmiAmount ?? dest.EmiAmount))
                  .ForMember(dest => dest.NextPaymentDueDate, opt => opt.MapFrom((src, dest) => src.NewNextPaymentDueDate ?? dest.NextPaymentDueDate))
                  .ForMember(dest => dest.OutstandingBalance, opt => opt.MapFrom((src, dest) =>
                      (src.PrincipalWriteOff.HasValue ? dest.OutstandingBalance - src.PrincipalWriteOff.Value : dest.OutstandingBalance)))
                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                  .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.RestructuredBy))
                  .ForMember(dest => dest.Notes, opt => opt.MapFrom((src, dest) =>
                      string.IsNullOrEmpty(src.Comments) ? dest.Notes :
                      $"{dest.Notes}\n[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] Restructured: {src.RestructuringReason}".Trim()))
                  .AfterMap((src, dest) =>
                  {
                      dest.StatusHistory.Add(new AccountStatusHistory
                      {
                          Status = AccountStatus.Restructured,
                          ChangedAt = DateTime.UtcNow,
                          ChangedBy = src.RestructuredBy,
                          Reason = src.RestructuringReason,
                          Comments = $"Type: {src.RestructuringType}. Payment holiday: {src.PaymentHolidayMonths ?? 0} months. {src.Comments}"
                      });
                  });
        }
    }
}
