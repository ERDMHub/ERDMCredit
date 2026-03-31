using ERDM.Core;
using ERDM.Core.Interfaces;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.Interfaces
{
    public interface ICreditApplicationRepository : IRepository<CreditApplication>
    {
        Task<CreditApplication> GetByApplicationIdAsync(string applicationId);
        Task<IEnumerable<CreditApplication>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<CreditApplication>> GetByStatusAsync(string status);
        Task<IEnumerable<CreditApplication>> GetPendingApplicationsAsync();
        Task<IEnumerable<CreditApplication>> GeCreditApplicationsAsync();
        Task<decimal> GetTotalApprovedAmountByCustomerAsync(string customerId);
        Task<Dictionary<string, int>> GetApplicationStatisticsByStatusAsync();
        Task<PaginatedResult<CreditApplication>> GetPaginatedByCustomerAsync(string customerId, int pageNumber, int pageSize, string sortBy = "CreatedAt",
            bool sortDescending = true);
    }
}
