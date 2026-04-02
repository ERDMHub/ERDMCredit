using ERDM.Core;
using ERDM.Core.Interfaces;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.Interfaces
{
    public interface ILimitHistoryRepository : IRepository<LimitHistory>
    {
        Task<LimitHistory?> GetByLimitHistoryIdAsync(string limitHistoryId);
        Task<IEnumerable<LimitHistory>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<LimitHistory>> GetByAccountIdAsync(string accountId);
        Task<IEnumerable<LimitHistory>> GetByChangeTypeAsync(LimitChangeType changeType);
        Task<IEnumerable<LimitHistory>> GetTemporaryLimitsAsync(bool activeOnly = true);
        Task<IEnumerable<LimitHistory>> GetExpiredTemporaryLimitsAsync(DateTime asOfDate);
        Task<LimitHistory?> GetLatestLimitChangeAsync(string accountId);
        Task<decimal> GetCurrentLimitAsync(string accountId);
        Task<Dictionary<string, int>> GetLimitStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<PaginatedResult<LimitHistory>> GetPaginatedWithFiltersAsync(LimitHistoryFilter filter, int pageNumber, int pageSize, string sortBy = "ChangedDate", bool sortDescending = true);
        Task<bool> UpdateExpiryDateAsync(string limitHistoryId, DateTime newExpiryDate, string updatedBy);
        Task<int> BulkExpireTemporaryLimitsAsync(DateTime asOfDate);
    }
}