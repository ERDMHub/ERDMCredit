using ERDM.Core;
using ERDM.Core.Interfaces;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Interfaces
{
    public interface ICreditDecisionRepository : IRepository<CreditDecision>
    {
        Task<CreditDecision?> GetByDecisionIdAsync(string decisionId);
        Task<CreditDecision?> GetByApplicationIdAsync(string applicationId);
        Task<IEnumerable<CreditDecision>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<CreditDecision>> GetByDecisionTypeAsync(DecisionType decisionType);
        Task<IEnumerable<CreditDecision>> GetByStatusAsync(DecisionStatus status);
        Task<IEnumerable<CreditDecision>> GetPendingApprovalsAsync();
        Task<IEnumerable<CreditDecision>> GetExpiredCounterOffersAsync();
        Task<decimal> GetTotalApprovedAmountByCustomerAsync(string customerId);
        Task<Dictionary<string, int>> GetDecisionStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<PaginatedResult<CreditDecision>> GetPaginatedWithFiltersAsync(CreditDecisionFilter filter, int pageNumber, int pageSize, string sortBy = "DecisionDate", bool sortDescending = true);
        Task<bool> UpdateDecisionStatusAsync(string decisionId, DecisionStatus newStatus, string updatedBy);
        Task<bool> AcceptCounterOfferAsync(string decisionId, string acceptedBy);
        Task<bool> UpdateConditionsAsync(string decisionId, List<UnderwritingCondition> conditions);
    }
}
