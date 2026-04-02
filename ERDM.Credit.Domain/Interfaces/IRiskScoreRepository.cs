using ERDM.Core;
using ERDM.Core.Interfaces;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Interfaces
{
    public interface IRiskScoreRepository : IRepository<RiskScore>
    {
        Task<RiskScore?> GetByRiskScoreIdAsync(string riskScoreId);
        Task<IEnumerable<RiskScore>> GetByCustomerIdAsync(string customerId);
        Task<RiskScore?> GetLatestByCustomerIdAsync(string customerId);
        Task<IEnumerable<RiskScore>> GetByApplicationIdAsync(string applicationId);
        Task<IEnumerable<RiskScore>> GetByAccountIdAsync(string accountId);
        Task<IEnumerable<RiskScore>> GetByScoreTypeAsync(ScoreType scoreType);
        Task<IEnumerable<RiskScore>> GetByRiskCategoryAsync(RiskCategory riskCategory);
        Task<IEnumerable<RiskScore>> GetValidScoresAsync();
        Task<IEnumerable<RiskScore>> GetExpiredScoresAsync(DateTime asOfDate);
        Task<IEnumerable<RiskScore>> GetScoresForReviewAsync(DateTime reviewDate);
        Task<Dictionary<string, int>> GetRiskScoreStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<PaginatedResult<RiskScore>> GetPaginatedWithFiltersAsync(RiskScoreFilter filter, int pageNumber, int pageSize, string sortBy = "ScoringDate", bool sortDescending = true);
        Task<bool> InvalidateScoreAsync(string riskScoreId, string invalidatedBy, string reason);
        Task<int> BulkUpdateValidityAsync(DateTime asOfDate);
    }
}