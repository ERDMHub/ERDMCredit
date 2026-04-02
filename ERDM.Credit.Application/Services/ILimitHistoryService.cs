using AutoMapper;
using ERDM.Credit.Contracts.DTOs.CreditDecisionDtos;
using ERDM.Credit.Contracts.DTOs.LimitHistoryDtos;
using ERDM.Credit.Contracts.Wrapper;
using ERDM.Credit.Domain.DomainEvents;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;
using ERDM.Credit.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERDM.Credit.Application.Services
{
    public interface ILimitHistoryService
    {
        // Create Operations
        Task<ApiResponse<LimitHistoryResponseDto>> CreateAsync(CreateLimitHistoryDto dto);
        
        // Read Operations
        Task<ApiResponse<LimitHistoryResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<LimitHistoryResponseDto>> GetByLimitHistoryIdAsync(string limitHistoryId);
        Task<ApiResponse<List<LimitHistoryResponseDto>>> GetByCustomerIdAsync(string customerId);
        Task<ApiResponse<List<LimitHistoryResponseDto>>> GetByAccountIdAsync(string accountId);
        Task<ApiResponse<PaginatedResponse<LimitHistoryResponseDto>>> GetAllAsync(LimitHistoryQueryDto query);
        Task<ApiResponse<LimitHistoryResponseDto>> GetLatestLimitChangeAsync(string accountId);
        Task<ApiResponse<decimal>> GetCurrentLimitAsync(string accountId);
        
        // Update Operations
        Task<ApiResponse<LimitHistoryResponseDto>> UpdateExpiryDateAsync(string id, ExtendTemporaryLimitDto dto);
        Task<ApiResponse<LimitHistoryResponseDto>> RevertLimitAsync(string id, RevertLimitDto dto);
        
        // Statistics
        Task<ApiResponse<LimitHistoryStatisticsDto>> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponse<CustomerLimitSummaryDto>> GetCustomerLimitSummaryAsync(string customerId);
        
        // Bulk Operations
        Task<ApiResponse<int>> BulkExpireTemporaryLimitsAsync();
    }
}