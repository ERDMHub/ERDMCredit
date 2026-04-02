using AutoMapper;
using ERDM.Credit.Contracts.DTOs.LimitHistoryDtos;
using ERDM.Credit.Contracts.Wrapper;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERDM.Credit.Application.Services
{
     public class LimitHistoryService : ILimitHistoryService
    {
        private readonly ILimitHistoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<LimitHistoryService> _logger;

        public LimitHistoryService(
            ILimitHistoryRepository repository,
            IMapper mapper,
            ILogger<LimitHistoryService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<LimitHistoryResponseDto>> CreateAsync(CreateLimitHistoryDto dto)
        {
            try
            {
                _logger.LogInformation("Creating limit history for account {AccountId}", dto.AccountId);

                var limitHistory = _mapper.Map<LimitHistory>(dto);
                var result = await _repository.AddAsync(limitHistory);

                var response = _mapper.Map<LimitHistoryResponseDto>(result);
                return ApiResponse<LimitHistoryResponseDto>.Ok(response, "Limit history created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating limit history for account {AccountId}", dto.AccountId);
                return ApiResponse<LimitHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LimitHistoryResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var limitHistory = await _repository.GetByIdAsync(id);
                if (limitHistory == null)
                    return ApiResponse<LimitHistoryResponseDto>.Fail($"Limit history with ID {id} not found");

                var response = _mapper.Map<LimitHistoryResponseDto>(limitHistory);
                return ApiResponse<LimitHistoryResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving limit history {Id}", id);
                return ApiResponse<LimitHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LimitHistoryResponseDto>> GetByLimitHistoryIdAsync(string limitHistoryId)
        {
            try
            {
                var limitHistory = await _repository.GetByLimitHistoryIdAsync(limitHistoryId);
                if (limitHistory == null)
                    return ApiResponse<LimitHistoryResponseDto>.Fail($"Limit history {limitHistoryId} not found");

                var response = _mapper.Map<LimitHistoryResponseDto>(limitHistory);
                return ApiResponse<LimitHistoryResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving limit history {LimitHistoryId}", limitHistoryId);
                return ApiResponse<LimitHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<LimitHistoryResponseDto>>> GetByCustomerIdAsync(string customerId)
        {
            try
            {
                var limitHistories = await _repository.GetByCustomerIdAsync(customerId);
                var response = _mapper.Map<List<LimitHistoryResponseDto>>(limitHistories);
                return ApiResponse<List<LimitHistoryResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving limit histories for customer {CustomerId}", customerId);
                return ApiResponse<List<LimitHistoryResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<LimitHistoryResponseDto>>> GetByAccountIdAsync(string accountId)
        {
            try
            {
                var limitHistories = await _repository.GetByAccountIdAsync(accountId);
                var response = _mapper.Map<List<LimitHistoryResponseDto>>(limitHistories);
                return ApiResponse<List<LimitHistoryResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving limit histories for account {AccountId}", accountId);
                return ApiResponse<List<LimitHistoryResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<LimitHistoryResponseDto>>> GetAllAsync(LimitHistoryQueryDto query)
        {
            try
            {
                var filter = new LimitHistoryFilter
                {
                    CustomerId = query.CustomerId,
                    AccountId = query.AccountId,
                    ChangeType = query.ChangeType != null ? Enum.Parse<LimitChangeType>(query.ChangeType) : null,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate,
                    ChangedBy = query.ChangedBy,
                    IsTemporary = query.IsTemporary
                };

                var result = await _repository.GetPaginatedWithFiltersAsync(
                    filter,
                    query.PageNumber,
                    query.PageSize,
                    query.SortBy,
                    query.SortDescending);

                var response = new PaginatedResponse<LimitHistoryResponseDto>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages,
                    HasPrevious = result.HasPrevious,
                    HasNext = result.HasNext,
                    Data = _mapper.Map<IEnumerable<LimitHistoryResponseDto>>(result.Data)
                };

                return ApiResponse<PaginatedResponse<LimitHistoryResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving limit histories");
                return ApiResponse<PaginatedResponse<LimitHistoryResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LimitHistoryResponseDto>> GetLatestLimitChangeAsync(string accountId)
        {
            try
            {
                var limitHistory = await _repository.GetLatestLimitChangeAsync(accountId);
                if (limitHistory == null)
                    return ApiResponse<LimitHistoryResponseDto>.Fail($"No limit history found for account {accountId}");

                var response = _mapper.Map<LimitHistoryResponseDto>(limitHistory);
                return ApiResponse<LimitHistoryResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest limit change for account {AccountId}", accountId);
                return ApiResponse<LimitHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<decimal>> GetCurrentLimitAsync(string accountId)
        {
            try
            {
                var currentLimit = await _repository.GetCurrentLimitAsync(accountId);
                return ApiResponse<decimal>.Ok(currentLimit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current limit for account {AccountId}", accountId);
                return ApiResponse<decimal>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LimitHistoryResponseDto>> UpdateExpiryDateAsync(string id, ExtendTemporaryLimitDto dto)
        {
            try
            {
                var limitHistory = await _repository.GetByIdAsync(id);
                if (limitHistory == null)
                    return ApiResponse<LimitHistoryResponseDto>.Fail($"Limit history with ID {id} not found");

                if (!limitHistory.IsTemporary)
                    return ApiResponse<LimitHistoryResponseDto>.Fail("Only temporary limits can be extended");

                // Call entity method to raise event
                limitHistory.UpdateExpiryDate(dto.NewExpiryDate, dto.ExtendedBy);
                
                await _repository.UpdateAsync(limitHistory);

                var response = _mapper.Map<LimitHistoryResponseDto>(limitHistory);
                return ApiResponse<LimitHistoryResponseDto>.Ok(response, "Temporary limit extended successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expiry date for limit history {Id}", id);
                return ApiResponse<LimitHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LimitHistoryResponseDto>> RevertLimitAsync(string id, RevertLimitDto dto)
        {
            try
            {
                var limitHistory = await _repository.GetByIdAsync(id);
                if (limitHistory == null)
                    return ApiResponse<LimitHistoryResponseDto>.Fail($"Limit history with ID {id} not found");

                // Call entity method to raise event
                limitHistory.RevertLimit(dto.RevertedLimit, dto.RevertedBy, dto.RevertReason);
                
                await _repository.UpdateAsync(limitHistory);

                var response = _mapper.Map<LimitHistoryResponseDto>(limitHistory);
                return ApiResponse<LimitHistoryResponseDto>.Ok(response, "Limit reverted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reverting limit {Id}", id);
                return ApiResponse<LimitHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LimitHistoryStatisticsDto>> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var limitHistories = await _repository.GetAllAsync();
                var filteredHistories = limitHistories.AsEnumerable();

                if (fromDate.HasValue)
                    filteredHistories = filteredHistories.Where(h => h.ChangedDate >= fromDate.Value);
                if (toDate.HasValue)
                    filteredHistories = filteredHistories.Where(h => h.ChangedDate <= toDate.Value);

                var historyList = filteredHistories.ToList();

                var increases = historyList.Where(h => h.ChangeType == LimitChangeType.Increase || h.ChangeType == LimitChangeType.TemporaryIncrease).ToList();
                var decreases = historyList.Where(h => h.ChangeType == LimitChangeType.Decrease || h.ChangeType == LimitChangeType.TemporaryDecrease).ToList();

                var response = new LimitHistoryStatisticsDto
                {
                    TotalChanges = historyList.Count,
                    Increases = increases.Count,
                    Decreases = decreases.Count,
                    TemporaryChanges = historyList.Count(h => h.IsTemporary),
                    PermanentChanges = historyList.Count(h => !h.IsTemporary),
                    TotalIncreaseAmount = increases.Sum(h => h.ChangeAmount),
                    TotalDecreaseAmount = Math.Abs(decreases.Sum(h => h.ChangeAmount)),
                    AverageIncreasePercentage = increases.Any() ? increases.Average(h => h.ChangePercentage) : 0,
                    AverageDecreasePercentage = decreases.Any() ? Math.Abs(decreases.Average(h => h.ChangePercentage)) : 0,
                    ChangesByReason = historyList
                        .Where(h => !string.IsNullOrEmpty(h.ReasonCode))
                        .GroupBy(h => h.ReasonCode)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    ChangesByType = historyList
                        .GroupBy(h => h.ChangeType.ToString())
                        .ToDictionary(g => g.Key, g => g.Count()),
                    MonthlyTrend = historyList
                        .GroupBy(h => h.ChangedDate.ToString("yyyy-MM"))
                        .ToDictionary(g => g.Key, g => g.Count()),
                    AsOfDate = DateTime.UtcNow
                };

                return ApiResponse<LimitHistoryStatisticsDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting limit history statistics");
                return ApiResponse<LimitHistoryStatisticsDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CustomerLimitSummaryDto>> GetCustomerLimitSummaryAsync(string customerId)
        {
            try
            {
                var limitHistories = await _repository.GetByCustomerIdAsync(customerId);
                var historyList = limitHistories.ToList();
                var latest = historyList.OrderByDescending(h => h.ChangedDate).FirstOrDefault();

                var activeTemporaryLimits = historyList
                    .Where(h => h.IsTemporary && (!h.ExpiryDate.HasValue || h.ExpiryDate > DateTime.UtcNow))
                    .Select(h => new ActiveTemporaryLimitDto
                    {
                        LimitHistoryId = h.LimitHistoryId,
                        AccountId = h.AccountId,
                        AccountNumber = h.AccountNumber,
                        TemporaryLimit = h.NewLimit,
                        OriginalLimit = h.PreviousLimit,
                        EffectiveDate = h.EffectiveDate,
                        ExpiryDate = h.ExpiryDate ?? DateTime.UtcNow,
                        DaysRemaining = h.ExpiryDate.HasValue ? (int)(h.ExpiryDate.Value - DateTime.UtcNow).TotalDays : 0,
                        Reason = h.Reason
                    }).ToList();

                var response = new CustomerLimitSummaryDto
                {
                    CustomerId = customerId,
                    CurrentTotalLimit = latest?.NewLimit ?? 0,
                    PreviousTotalLimit = latest?.PreviousLimit ?? 0,
                    TotalLimitChanges = historyList.Count,
                    LastLimitChangeDate = latest?.ChangedDate ?? DateTime.UtcNow,
                    LastChangeType = latest?.ChangeType.ToString(),
                    RecentChanges = _mapper.Map<List<LimitHistorySummaryItemDto>>(historyList.Take(10)),
                    ActiveTemporaryLimits = activeTemporaryLimits
                };

                return ApiResponse<CustomerLimitSummaryDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer limit summary for {CustomerId}", customerId);
                return ApiResponse<CustomerLimitSummaryDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> BulkExpireTemporaryLimitsAsync()
        {
            try
            {
                var count = await _repository.BulkExpireTemporaryLimitsAsync(DateTime.UtcNow);
                return ApiResponse<int>.Ok(count, $"Expired {count} temporary limits");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk expiring temporary limits");
                return ApiResponse<int>.Fail(ex.Message);
            }
        }
    }
}