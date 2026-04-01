using ERDM.Core;
using ERDM.Core.Interfaces;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByAccountIdAsync(string accountId);
        Task<Account> GetByAccountNumberAsync(string accountNumber);
        Task<IEnumerable<Account>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Account>> GetByApplicationIdAsync(string applicationId);

        // Status-based queries
        Task<IEnumerable<Account>> GetByStatusAsync(AccountStatus status);
        Task<IEnumerable<Account>> GetActiveAccountsAsync();
        Task<IEnumerable<Account>> GetDelinquentAccountsAsync();
        Task<IEnumerable<Account>> GetAccountsDueForPaymentAsync(DateTime cutoffDate);
        Task<IEnumerable<Account>> GetOverdueAccountsAsync();
        Task<IEnumerable<Account>> GetMaturedAccountsAsync();

        // Type-based queries
        Task<IEnumerable<Account>> GetByAccountTypeAsync(AccountType accountType);
        Task<IEnumerable<Account>> GetByProductTypeAsync(string productType);

        // Financial queries
        Task<decimal> GetTotalOutstandingBalanceByCustomerAsync(string customerId);
        Task<decimal> GetTotalDisbursedAmountByCustomerAsync(string customerId);
        Task<decimal> GetAvailableCreditByCustomerAsync(string customerId);
        Task<Dictionary<AccountStatus, decimal>> GetBalanceSummaryByStatusAsync();
        Task<Dictionary<AccountType, decimal>> GetBalanceSummaryByTypeAsync();

        // Statistics and analytics
        Task<Dictionary<AccountStatus, int>> GetAccountStatisticsByStatusAsync();
        Task<Dictionary<AccountType, int>> GetAccountStatisticsByTypeAsync();

        // Payment-related
        Task<Account> GetAccountWithPaymentsAsync(string accountId);
        Task<IEnumerable<PaymentHistory>> GetPaymentHistoryAsync(string accountId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PaymentHistory?> GetLastPaymentAsync(string accountId);
        Task<decimal> GetTotalPaymentsByAccountAsync(string accountId);
        Task<decimal> GetTotalInterestEarnedByAccountAsync(string accountId);

        // Paginated results
        Task<PaginatedResult<Account>> GetPaginatedByCustomerAsync(
            string customerId,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true);

        Task<PaginatedResult<Account>> GetPaginatedByStatusAsync(
            AccountStatus status,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true);

        Task<PaginatedResult<Account>> GetPaginatedByDateRangeAsync(
            DateTime fromDate,
            DateTime toDate,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true);

        // Update operations
        Task<bool> UpdateAccountStatusAsync(string accountId, AccountStatus newStatus, string reason, string updatedBy);
        Task<bool> UpdateOutstandingBalanceAsync(string accountId, decimal newBalance);
        Task<bool> UpdateAvailableCreditAsync(string accountId, decimal newAvailableCredit);
        Task<bool> AddPaymentHistoryAsync(string accountId, PaymentHistory payment);
        Task<bool> RecordLatePaymentAsync(string accountId, int lateDays, decimal lateFee);

        // Bulk operations
        Task<bool> BulkUpdateStatusAsync(List<string> accountIds, AccountStatus newStatus, string reason, string updatedBy);
        Task<int> BulkMarkOverdueAccountsAsync(DateTime asOfDate);
        Task<int> UpdateNextPaymentDatesAsync();

        // Reporting
        Task<List<Account>> GetAccountsForCollectionAsync(int daysOverdue);
        Task<Dictionary<string, object>> GetAccountPerformanceMetricsAsync(string accountId);

        // Validation
        Task<bool> AccountExistsAsync(string accountNumber);
        Task<bool> IsAccountActiveAsync(string accountId);
        Task<bool> HasOutstandingBalanceAsync(string accountId);
        Task<DateTime?> GetNextPaymentDueDateAsync(string accountId);
    }
}
