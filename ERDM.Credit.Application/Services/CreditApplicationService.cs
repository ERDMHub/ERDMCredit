using AutoMapper;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Contracts.Wrapper;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ERDM.Credit.Application.Services
{
    public class CreditApplicationService : ICreditApplicationService
    {
        private readonly ICreditApplicationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreditApplicationService> _logger;

        public CreditApplicationService(
            ICreditApplicationRepository repository,
            IMapper mapper,
            ILogger<CreditApplicationService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CreditApplicationResponseDto>> CreateAsync(CreateCreditApplicationDto dto)
        {
            try
            {
                _logger.LogInformation("Creating credit application for customer {CustomerId}", dto.CustomerId);

                var application = _mapper.Map<CreditApplication>(dto);
                var result = await _repository.AddAsync(application);

                var response = _mapper.Map<CreditApplicationResponseDto>(result);
                return ApiResponse<CreditApplicationResponseDto>.Ok(response, "Application created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating application for customer {CustomerId}", dto.CustomerId);
                return ApiResponse<CreditApplicationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditApplicationResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var application = await _repository.GetByIdAsync(id);
                if (application == null)
                    return ApiResponse<CreditApplicationResponseDto>.Fail($"Application with ID {id} not found");

                var response = _mapper.Map<CreditApplicationResponseDto>(application);
                return ApiResponse<CreditApplicationResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application {Id}", id);
                return ApiResponse<CreditApplicationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditApplicationResponseDto>> GetByApplicationNumberAsync(string applicationNumber)
        {
            try
            {
                var application = await _repository.GetByApplicationIdAsync(applicationNumber);
                if (application == null)
                    return ApiResponse<CreditApplicationResponseDto>.Fail($"Application {applicationNumber} not found");

                var response = _mapper.Map<CreditApplicationResponseDto>(application);
                return ApiResponse<CreditApplicationResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application {Number}", applicationNumber);
                return ApiResponse<CreditApplicationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>> GetAllAsync(ApplicationQueryDto query)
        {
            try
            {
                // Build the filter expression
                var filter = BuildFilter(query);

                // Build the sort expression from string, use null if not specified to let repository use default
                Expression<Func<CreditApplication, object>> sortExpression = null;

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    sortExpression = query.SortBy.ToLower() switch
                    {
                        "applicationid" => x => x.ApplicationId,
                        "customerid" => x => x.CustomerId,
                        "producttype" => x => x.ProductType,
                        "requestedamount" => x => x.RequestedAmount,
                        "requestedterm" => x => x.RequestedTerm,
                        "status" => x => x.Status,
                        "createdat" => x => x.CreatedAt,
                        "updatedat" => x => x.UpdatedAt,
                        "expiresat" => x => x.ExpiresAt,
                        _ => x => x.CreatedAt // Default sort field
                    };
                }

                var result = await _repository.GetPaginatedAsync(
                    query.Page,
                    query.PageSize,
                    filter,
                    sortExpression, // Pass null to use repository's default sort (CreatedAt descending)
                    query.SortDescending);

                var response = new PaginatedResponse<CreditApplicationResponseDto>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages,
                    HasPrevious = result.HasPrevious,
                    HasNext = result.HasNext,
                    Data = _mapper.Map<IEnumerable<CreditApplicationResponseDto>>(result.Data)
                };

                return ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applications");
                return ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditApplicationResponseDto>> SubmitAsync(string id)
        {
            try
            {
                var application = await _repository.GetByIdAsync(id);
                if (application == null)
                    return ApiResponse<CreditApplicationResponseDto>.Fail($"Application with ID {id} not found");

                application.Submit();
                await _repository.UpdateAsync(application);

                var response = _mapper.Map<CreditApplicationResponseDto>(application);
                return ApiResponse<CreditApplicationResponseDto>.Ok(response, "Application submitted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting application {Id}", id);
                return ApiResponse<CreditApplicationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditApplicationResponseDto>> ApproveAsync(string id, ApproveApplicationDto dto)
        {
            try
            {
                var application = await _repository.GetByIdAsync(id);
                if (application == null)
                    return ApiResponse<CreditApplicationResponseDto>.Fail($"Application with ID {id} not found");

                application.Approve(dto.ApprovedAmount, dto.InterestRate, dto.RiskGrade,
                    dto.ReasonCodes, dto.DecidedBy);
                await _repository.UpdateAsync(application);

                var response = _mapper.Map<CreditApplicationResponseDto>(application);
                return ApiResponse<CreditApplicationResponseDto>.Ok(response, "Application approved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving application {Id}", id);
                return ApiResponse<CreditApplicationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditApplicationResponseDto>> DeclineAsync(string id, DeclineApplicationDto dto)
        {
            try
            {
                var application = await _repository.GetByIdAsync(id);
                if (application == null)
                    return ApiResponse<CreditApplicationResponseDto>.Fail($"Application with ID {id} not found");

                application.Decline(dto.DeclineReasons, dto.DecidedBy);
                await _repository.UpdateAsync(application);

                var response = _mapper.Map<CreditApplicationResponseDto>(application);
                return ApiResponse<CreditApplicationResponseDto>.Ok(response, "Application declined");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error declining application {Id}", id);
                return ApiResponse<CreditApplicationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<Dictionary<string, int>>> GetStatisticsAsync()
        {
            try
            {
                var statistics = await _repository.GetApplicationStatisticsByStatusAsync();
                return ApiResponse<Dictionary<string, int>>.Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics");
                return ApiResponse<Dictionary<string, int>>.Fail(ex.Message);
            }
        }

        private System.Linq.Expressions.Expression<Func<CreditApplication, bool>> BuildFilter(ApplicationQueryDto query)
        {
            return x => (string.IsNullOrEmpty(query.Status) || x.Status == query.Status) &&
                        (string.IsNullOrEmpty(query.CustomerId) || x.CustomerId == query.CustomerId);
        }

        public async Task<ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>> GetAllAsync()
        {
            try
            {
              

                var result = await _repository.GeCreditApplicationsAsync();

                var response = new PaginatedResponse<CreditApplicationResponseDto>
                {
                    PageNumber =100,
                    PageSize = 100,
                    TotalCount = result.Count(),
                    TotalPages = 100,
                    HasPrevious = false,
                    HasNext = false,
                    Data = _mapper.Map<IEnumerable<CreditApplicationResponseDto>>(result)
                };

                return ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applications");
                return ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>.Fail(ex.Message);
            }
        }
    }
}