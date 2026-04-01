using AutoMapper;
using ERDM.Credit.Contracts.DTOs.CreditDecisionDtos;
using ERDM.Credit.Contracts.Wrapper;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;
using ERDM.Credit.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERDM.Credit.Application.Services
{
    public class CreditDecisionService : ICreditDecisionService
    {
        private readonly ICreditDecisionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreditDecisionService> _logger;

        public CreditDecisionService(
            ICreditDecisionRepository repository,
            IMapper mapper,
            ILogger<CreditDecisionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> CreateAsync(CreateCreditDecisionDto dto)
        {
            try
            {
                _logger.LogInformation("Creating credit decision for application {ApplicationId}", dto.ApplicationId);

                var existingDecision = await _repository.GetByApplicationIdAsync(dto.ApplicationId);
                if (existingDecision != null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Decision already exists for application {dto.ApplicationId}");

                var decision = _mapper.Map<CreditDecision>(dto);
                var result = await _repository.AddAsync(decision);

                var response = _mapper.Map<CreditDecisionResponseDto>(result);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response, "Credit decision created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating credit decision for application {ApplicationId}", dto.ApplicationId);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision with ID {id} not found");

                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving credit decision {Id}", id);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> GetByDecisionIdAsync(string decisionId)
        {
            try
            {
                var decision = await _repository.GetByDecisionIdAsync(decisionId);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision {decisionId} not found");

                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving credit decision {DecisionId}", decisionId);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> GetByApplicationIdAsync(string applicationId)
        {
            try
            {
                var decision = await _repository.GetByApplicationIdAsync(applicationId);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision for application {applicationId} not found");

                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving credit decision for application {ApplicationId}", applicationId);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CreditDecisionResponseDto>>> GetByCustomerIdAsync(string customerId)
        {
            try
            {
                var decisions = await _repository.GetByCustomerIdAsync(customerId);
                var response = _mapper.Map<List<CreditDecisionResponseDto>>(decisions);
                return ApiResponse<List<CreditDecisionResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving credit decisions for customer {CustomerId}", customerId);
                return ApiResponse<List<CreditDecisionResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<CreditDecisionResponseDto>>> GetAllAsync(CreditDecisionQueryDto query)
        {
            try
            {
                var filter = new CreditDecisionFilter
                {
                    ApplicationId = query.ApplicationId,
                    CustomerId = query.CustomerId,
                    DecisionType = query.DecisionType != null ? Enum.Parse<DecisionType>(query.DecisionType) : null,
                    Status = query.Status != null ? Enum.Parse<DecisionStatus>(query.Status) : null,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate,
                    DecisionBy = query.DecisionBy,
                    RiskGrade = query.RiskGrade
                };

                var result = await _repository.GetPaginatedWithFiltersAsync(
                    filter,
                    query.PageNumber,
                    query.PageSize,
                    query.SortBy,
                    query.SortDescending);

                var response = new PaginatedResponse<CreditDecisionResponseDto>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages,
                    HasPrevious = result.HasPrevious,
                    HasNext = result.HasNext,
                    Data = _mapper.Map<IEnumerable<CreditDecisionResponseDto>>(result.Data)
                };

                return ApiResponse<PaginatedResponse<CreditDecisionResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving credit decisions");
                return ApiResponse<PaginatedResponse<CreditDecisionResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> ApproveDecisionAsync(string id, ApproveCreditDecisionDto dto)
        {
            try
            {
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision with ID {id} not found");

                _mapper.Map(dto, decision);
                await _repository.UpdateAsync(decision);

                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response, "Decision approved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving decision {Id}", id);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> DeclineDecisionAsync(string id, DeclineCreditDecisionDto dto)
        {
            try
            {
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision with ID {id} not found");

                _mapper.Map(dto, decision);
                await _repository.UpdateAsync(decision);

                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response, "Decision declined successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error declining decision {Id}", id);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> AcceptCounterOfferAsync(string id, AcceptCounterOfferDto dto)
        {
            try
            {
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision with ID {id} not found");

                if (!decision.IsCounterOffer)
                    return ApiResponse<CreditDecisionResponseDto>.Fail("This decision is not a counter offer");

                var success = await _repository.AcceptCounterOfferAsync(decision.DecisionId, dto.AcceptedBy);
                if (!success)
                    return ApiResponse<CreditDecisionResponseDto>.Fail("Failed to accept counter offer");

                decision = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response, "Counter offer accepted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting counter offer {Id}", id);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> UpdateDecisionStatusAsync(string id, string status, string updatedBy)
        {
            try
            {
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision with ID {id} not found");

                var newStatus = Enum.Parse<DecisionStatus>(status);
                var success = await _repository.UpdateDecisionStatusAsync(decision.DecisionId, newStatus, updatedBy);

                if (!success)
                    return ApiResponse<CreditDecisionResponseDto>.Fail("Failed to update decision status");

                decision = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response, $"Decision status updated to {status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating decision status {Id}", id);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionResponseDto>> UpdateConditionsAsync(string id, List<UpdateUnderwritingConditionDto> conditions)
        {
            try
            {
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return ApiResponse<CreditDecisionResponseDto>.Fail($"Credit decision with ID {id} not found");

                var updatedConditions = _mapper.Map<List<UnderwritingCondition>>(conditions);
                var success = await _repository.UpdateConditionsAsync(decision.DecisionId, updatedConditions);

                if (!success)
                    return ApiResponse<CreditDecisionResponseDto>.Fail("Failed to update conditions");

                decision = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<CreditDecisionResponseDto>(decision);
                return ApiResponse<CreditDecisionResponseDto>.Ok(response, "Conditions updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating conditions for decision {Id}", id);
                return ApiResponse<CreditDecisionResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> MetConditionAsync(string decisionId, string conditionId, string metBy)
        {
            try
            {
                var decision = await _repository.GetByDecisionIdAsync(decisionId);
                if (decision == null)
                    return ApiResponse<bool>.Fail($"Credit decision {decisionId} not found");

                var condition = decision.Conditions?.FirstOrDefault(c => c.ConditionId == conditionId);
                if (condition == null)
                    return ApiResponse<bool>.Fail($"Condition {conditionId} not found");

                condition.IsMet = true;
                condition.MetDate = DateTime.UtcNow;
                condition.MetBy = metBy;

                var success = await _repository.UpdateConditionsAsync(decisionId, decision.Conditions ?? new List<UnderwritingCondition>());
                return ApiResponse<bool>.Ok(success, success ? "Condition marked as met" : "Failed to update condition");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking condition {ConditionId} as met for decision {DecisionId}", conditionId, decisionId);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CreditDecisionStatisticsDto>> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var decisions = await _repository.GetAllAsync();
                var filteredDecisions = decisions.AsEnumerable();

                if (fromDate.HasValue)
                    filteredDecisions = filteredDecisions.Where(d => d.DecisionDate >= fromDate.Value);
                if (toDate.HasValue)
                    filteredDecisions = filteredDecisions.Where(d => d.DecisionDate <= toDate.Value);

                var decisionList = filteredDecisions.ToList();

                var response = new CreditDecisionStatisticsDto
                {
                    TotalDecisions = decisionList.Count,
                    Approved = decisionList.Count(d => d.DecisionType == DecisionType.Approved),
                    Declined = decisionList.Count(d => d.DecisionType == DecisionType.Declined),
                    CounterOffers = decisionList.Count(d => d.IsCounterOffer),
                    Pending = decisionList.Count(d => d.Status == DecisionStatus.Pending),
                    Referred = decisionList.Count(d => d.DecisionType == DecisionType.Referred),
                    TotalApprovedAmount = decisionList.Where(d => d.ApprovedAmount.HasValue).Sum(d => d.ApprovedAmount.Value),
                    AverageApprovedAmount = decisionList.Where(d => d.ApprovedAmount.HasValue).Any()
                        ? decisionList.Where(d => d.ApprovedAmount.HasValue).Average(d => d.ApprovedAmount.Value)
                        : 0,
                    AverageInterestRate = decisionList.Where(d => d.ApprovedInterestRate.HasValue).Any()
                        ? decisionList.Where(d => d.ApprovedInterestRate.HasValue).Average(d => d.ApprovedInterestRate.Value)
                        : 0,
                    DecisionsByRiskGrade = decisionList
                        .Where(d => !string.IsNullOrEmpty(d.RiskGrade))
                        .GroupBy(d => d.RiskGrade!)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    DecisionsByProductType = decisionList
                        .Where(d => !string.IsNullOrEmpty(d.ApprovedProductType))
                        .GroupBy(d => d.ApprovedProductType!)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    DecisionsByDay = decisionList
                        .GroupBy(d => d.DecisionDate.Date)
                        .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Count()),
                    AsOfDate = DateTime.UtcNow
                };

                return ApiResponse<CreditDecisionStatisticsDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting credit decision statistics");
                return ApiResponse<CreditDecisionStatisticsDto>.Fail(ex.Message);
            }
        }
    }
}