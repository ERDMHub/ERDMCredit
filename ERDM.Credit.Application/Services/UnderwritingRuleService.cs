using AutoMapper;
using ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos;
using ERDM.Credit.Contracts.Wrapper;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;
using ERDM.Credit.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERDM.Credit.Application.Services
{
    public class UnderwritingRuleService : IUnderwritingRuleService
    {
        private readonly IUnderwritingRuleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UnderwritingRuleService> _logger;

        public UnderwritingRuleService(
            IUnderwritingRuleRepository repository,
            IMapper mapper,
            ILogger<UnderwritingRuleService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Create Operations

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> CreateAsync(CreateUnderwritingRuleDto dto)
        {
            try
            {
                _logger.LogInformation("Creating underwriting rule {RuleName}", dto.RuleName);

                // Check if rule code already exists
                var existingRule = await _repository.GetByRuleCodeAsync(dto.RuleCode);
                if (existingRule != null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Rule with code {dto.RuleCode} already exists");

                var rule = _mapper.Map<UnderwritingRule>(dto);

                // Raise domain event
                rule.RaiseCreatedEvent();

                var result = await _repository.AddAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(result);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Underwriting rule created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating underwriting rule {RuleName}", dto.RuleName);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Read Operations

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving underwriting rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> GetByRuleIdAsync(string ruleId)
        {
            try
            {
                var rule = await _repository.GetByRuleIdAsync(ruleId);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule {ruleId} not found");

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving underwriting rule {RuleId}", ruleId);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> GetByRuleCodeAsync(string ruleCode)
        {
            try
            {
                var rule = await _repository.GetByRuleCodeAsync(ruleCode);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with code {ruleCode} not found");

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving underwriting rule {RuleCode}", ruleCode);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<UnderwritingRuleResponseDto>>> GetAllAsync(UnderwritingRuleQueryDto query)
        {
            try
            {
                var filter = new UnderwritingRuleFilter
                {
                    RuleName = query.RuleName,
                    RuleCode = query.RuleCode,
                    RuleType = query.RuleType != null ? Enum.Parse<RuleType>(query.RuleType) : null,
                    Category = query.Category != null ? Enum.Parse<RuleCategory>(query.Category) : null,
                    Status = query.Status != null ? Enum.Parse<RuleStatus>(query.Status) : null,
                    IsActive = query.IsActive,
                    CreatedBy = query.CreatedBy,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate
                };

                var result = await _repository.GetPaginatedWithFiltersAsync(
                    filter,
                    query.PageNumber,
                    query.PageSize,
                    query.SortBy,
                    query.SortDescending);

                var response = new PaginatedResponse<UnderwritingRuleResponseDto>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages,
                    HasPrevious = result.HasPrevious,
                    HasNext = result.HasNext,
                    Data = _mapper.Map<IEnumerable<UnderwritingRuleResponseDto>>(result.Data)
                };

                return ApiResponse<PaginatedResponse<UnderwritingRuleResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving underwriting rules");
                return ApiResponse<PaginatedResponse<UnderwritingRuleResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<UnderwritingRuleResponseDto>>> GetActiveRulesAsync()
        {
            try
            {
                var rules = await _repository.GetActiveRulesAsync();
                var response = _mapper.Map<List<UnderwritingRuleResponseDto>>(rules);
                return ApiResponse<List<UnderwritingRuleResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active underwriting rules");
                return ApiResponse<List<UnderwritingRuleResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<UnderwritingRuleResponseDto>>> GetRulesByTypeAsync(string ruleType)
        {
            try
            {
                var type = Enum.Parse<RuleType>(ruleType);
                var rules = await _repository.GetByRuleTypeAsync(type);
                var response = _mapper.Map<List<UnderwritingRuleResponseDto>>(rules);
                return ApiResponse<List<UnderwritingRuleResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving underwriting rules by type {RuleType}", ruleType);
                return ApiResponse<List<UnderwritingRuleResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<UnderwritingRuleResponseDto>>> GetRulesByCategoryAsync(string category)
        {
            try
            {
                var cat = Enum.Parse<RuleCategory>(category);
                var rules = await _repository.GetByCategoryAsync(cat);
                var response = _mapper.Map<List<UnderwritingRuleResponseDto>>(rules);
                return ApiResponse<List<UnderwritingRuleResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving underwriting rules by category {Category}", category);
                return ApiResponse<List<UnderwritingRuleResponseDto>>.Fail(ex.Message);
            }
        }

        #endregion

        #region Update Operations

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> UpdateRuleAsync(string id, UpdateUnderwritingRuleDto dto)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                var updateData = _mapper.Map<UpdateRuleData>(dto);

                // This calls UpdateRule which raises UnderwritingRuleUpdatedEvent
                rule.UpdateRule(updateData, dto.UpdatedBy);

                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Rule updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating underwriting rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> ActivateRuleAsync(string id, string activatedBy)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                // Validate dependencies before activation
                if (rule.DependsOnRules.Any())
                {
                    var isValid = await _repository.ValidateRuleDependenciesAsync(rule.RuleId, rule.DependsOnRules);
                    if (!isValid)
                    {
                        return ApiResponse<UnderwritingRuleResponseDto>.Fail("Circular dependency detected. Cannot activate rule.");
                    }
                }

                // This calls Activate which raises UnderwritingRuleActivatedEvent
                rule.Activate(activatedBy);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Rule activated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating underwriting rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> DeactivateRuleAsync(string id, string deactivatedBy, string reason)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                // This calls Deactivate which raises UnderwritingRuleDeactivatedEvent
                rule.Deactivate(deactivatedBy, reason);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Rule deactivated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating underwriting rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> ApproveRuleAsync(string id, string approvedBy)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                // This calls Approve which raises UnderwritingRuleApprovedEvent
                rule.Approve(approvedBy);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Rule approved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving underwriting rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> RejectRuleAsync(string id, string rejectedBy, string reason)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                // This calls Reject which raises UnderwritingRuleRejectedEvent
                rule.Reject(rejectedBy, reason);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Rule rejected successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting underwriting rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Execution Operations

        public async Task<ApiResponse<RuleExecutionResultDto>> ExecuteRuleAsync(string ruleId, ExecuteRuleRequestDto request)
        {
            try
            {
                var rule = await _repository.GetByRuleIdAsync(ruleId);
                if (rule == null)
                    return ApiResponse<RuleExecutionResultDto>.Fail($"Rule {ruleId} not found");

                if (rule.Status != RuleStatus.Active)
                    return ApiResponse<RuleExecutionResultDto>.Fail($"Rule {ruleId} is not active");

                // Check if rule has expired
                if (rule.EffectiveTo.HasValue && rule.EffectiveTo.Value < DateTime.UtcNow)
                {
                    return ApiResponse<RuleExecutionResultDto>.Fail($"Rule {ruleId} has expired");
                }

                var startTime = DateTime.UtcNow;
                bool isTrue = false;
                string? failureReason = null;

                try
                {
                    // Evaluate the condition expression
                    isTrue = EvaluateCondition(rule.ConditionExpression, request.InputData);
                }
                catch (Exception ex)
                {
                    failureReason = ex.Message;
                    isTrue = false;
                }

                var executionTime = DateTime.UtcNow - startTime;
                var outcome = isTrue ? rule.TrueOutcome : (rule.FalseOutcome ?? new RuleOutcome { OutcomeType = "Default", Message = "Condition not met" });

                // This calls RecordExecution which raises UnderwritingRuleExecutedEvent
                rule.RecordExecution(isTrue, failureReason);
                await _repository.UpdateAsync(rule);

                var result = new RuleExecutionResultDto
                {
                    RuleId = rule.RuleId,
                    RuleName = rule.RuleName,
                    IsTrue = isTrue,
                    Outcome = _mapper.Map<RuleOutcomeDto>(outcome),
                    ActionsExecuted = _mapper.Map<List<RuleActionDto>>(isTrue ? rule.Actions : new List<RuleAction>()),
                    ExecutionTime = executionTime,
                    ExecutedAt = DateTime.UtcNow
                };

                return ApiResponse<RuleExecutionResultDto>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing rule {RuleId}", ruleId);
                return ApiResponse<RuleExecutionResultDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RuleExecutionResultDto>>> ExecuteRuleSetAsync(List<string> ruleIds, ExecuteRuleRequestDto request)
        {
            try
            {
                var results = new List<RuleExecutionResultDto>();
                var executedRules = new HashSet<string>();

                foreach (var ruleId in ruleIds.OrderBy(id => id))
                {
                    if (executedRules.Contains(ruleId))
                        continue;

                    var result = await ExecuteRuleAsync(ruleId, request);
                    if (result.Success && result.Data != null)
                    {
                        results.Add(result.Data);
                        executedRules.Add(ruleId);

                        // Add next rules from outcome
                        foreach (var nextRule in result.Data.Outcome.NextRules)
                        {
                            if (!executedRules.Contains(nextRule) && !ruleIds.Contains(nextRule))
                                ruleIds.Add(nextRule);
                        }
                    }
                }

                return ApiResponse<List<RuleExecutionResultDto>>.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing rule set");
                return ApiResponse<List<RuleExecutionResultDto>>.Fail(ex.Message);
            }
        }

        #endregion

        #region Dependency Operations

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> AddDependencyAsync(string id, string dependencyRuleId, string addedBy)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                var dependencyRule = await _repository.GetByRuleIdAsync(dependencyRuleId);
                if (dependencyRule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Dependency rule {dependencyRuleId} not found");

                // This calls AddDependency which raises UnderwritingRuleDependencyAddedEvent
                rule.AddDependency(dependencyRuleId);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Dependency added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding dependency to rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> RemoveDependencyAsync(string id, string dependencyRuleId, string removedBy)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                // This calls RemoveDependency which raises UnderwritingRuleDependencyRemovedEvent
                rule.RemoveDependency(dependencyRuleId);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Dependency removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing dependency from rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Schedule Operations

        public async Task<ApiResponse<UnderwritingRuleResponseDto>> ScheduleRuleAsync(string id, DateTime effectiveFrom, DateTime? effectiveTo, string scheduledBy)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<UnderwritingRuleResponseDto>.Fail($"Underwriting rule with ID {id} not found");

                // This calls ScheduleEffectiveDate which raises UnderwritingRuleScheduledEvent
                rule.ScheduleEffectiveDate(effectiveFrom, effectiveTo, scheduledBy);
                await _repository.UpdateAsync(rule);

                var response = _mapper.Map<UnderwritingRuleResponseDto>(rule);
                return ApiResponse<UnderwritingRuleResponseDto>.Ok(response, "Rule scheduled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling rule {Id}", id);
                return ApiResponse<UnderwritingRuleResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Statistics and Reports

        public async Task<ApiResponse<UnderwritingRuleStatisticsDto>> GetStatisticsAsync()
        {
            try
            {
                var rules = await _repository.GetAllAsync();
                var ruleList = rules.ToList();
                var activeRules = ruleList.Where(r => r.Status == RuleStatus.Active).ToList();

                var stats = new UnderwritingRuleStatisticsDto
                {
                    TotalRules = ruleList.Count,
                    ActiveRules = activeRules.Count,
                    InactiveRules = ruleList.Count(r => r.Status == RuleStatus.Inactive),
                    DraftRules = ruleList.Count(r => r.Status == RuleStatus.Draft),
                    ApprovedRules = ruleList.Count(r => r.Status == RuleStatus.Approved),
                    RulesByType = ruleList.GroupBy(r => r.RuleType.ToString()).ToDictionary(g => g.Key, g => g.Count()),
                    RulesByCategory = ruleList.GroupBy(r => r.Category.ToString()).ToDictionary(g => g.Key, g => g.Count()),
                    AverageSuccessRate = activeRules.Any() ? activeRules.Average(r => r.SuccessRate) : 0,
                    TotalExecutions = activeRules.Sum(r => r.ExecutionCount),
                    TopPerformingRules = activeRules
                        .Where(r => r.ExecutionCount > 0)
                        .OrderByDescending(r => r.SuccessRate)
                        .Take(5)
                        .Select(r => new RuleExecutionStatsDto
                        {
                            RuleId = r.RuleId,
                            RuleName = r.RuleName,
                            ExecutionCount = r.ExecutionCount,
                            SuccessRate = r.SuccessRate,
                            LastExecutedAt = r.LastExecutedAt
                        }).ToList(),
                    UnderperformingRules = activeRules
                        .Where(r => r.ExecutionCount > 0 && r.SuccessRate < 50)
                        .OrderBy(r => r.SuccessRate)
                        .Take(5)
                        .Select(r => new RuleExecutionStatsDto
                        {
                            RuleId = r.RuleId,
                            RuleName = r.RuleName,
                            ExecutionCount = r.ExecutionCount,
                            SuccessRate = r.SuccessRate,
                            LastExecutedAt = r.LastExecutedAt
                        }).ToList(),
                    AsOfDate = DateTime.UtcNow
                };

                return ApiResponse<UnderwritingRuleStatisticsDto>.Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting underwriting rule statistics");
                return ApiResponse<UnderwritingRuleStatisticsDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RuleExecutionStatsDto>>> GetTopPerformingRulesAsync(int count = 10)
        {
            try
            {
                var rules = await _repository.GetActiveRulesAsync();
                var topRules = rules
                    .Where(r => r.ExecutionCount > 0)
                    .OrderByDescending(r => r.SuccessRate)
                    .Take(count)
                    .Select(r => new RuleExecutionStatsDto
                    {
                        RuleId = r.RuleId,
                        RuleName = r.RuleName,
                        ExecutionCount = r.ExecutionCount,
                        SuccessRate = r.SuccessRate,
                        LastExecutedAt = r.LastExecutedAt
                    }).ToList();

                return ApiResponse<List<RuleExecutionStatsDto>>.Ok(topRules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top performing rules");
                return ApiResponse<List<RuleExecutionStatsDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RuleExecutionStatsDto>>> GetUnderperformingRulesAsync(int count = 10)
        {
            try
            {
                var rules = await _repository.GetActiveRulesAsync();
                var underperforming = rules
                    .Where(r => r.ExecutionCount > 0 && r.SuccessRate < 50)
                    .OrderBy(r => r.SuccessRate)
                    .Take(count)
                    .Select(r => new RuleExecutionStatsDto
                    {
                        RuleId = r.RuleId,
                        RuleName = r.RuleName,
                        ExecutionCount = r.ExecutionCount,
                        SuccessRate = r.SuccessRate,
                        LastExecutedAt = r.LastExecutedAt
                    }).ToList();

                return ApiResponse<List<RuleExecutionStatsDto>>.Ok(underperforming);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting underperforming rules");
                return ApiResponse<List<RuleExecutionStatsDto>>.Fail(ex.Message);
            }
        }

        #endregion

        #region Validation Operations

        public async Task<ApiResponse<RuleValidationResultDto>> ValidateRuleAsync(string id)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<RuleValidationResultDto>.Fail($"Rule with ID {id} not found");

                var result = new RuleValidationResultDto
                {
                    IsValid = true,
                    Errors = new List<string>(),
                    Warnings = new List<string>()
                };

                // Validate condition expression syntax
                try
                {
                    var testData = new Dictionary<string, object>
                    {
                        { "creditScore", 700 },
                        { "income", 50000 },
                        { "debtToIncome", 0.35m },
                        { "age", 30 },
                        { "requestedAmount", 10000 },
                        { "employmentYears", 3 },
                        { "collateralValue", 15000 },
                        { "fraudScore", 20 },
                        { "country", "USA" },
                        { "employmentType", "Employed" }
                    };
                    EvaluateCondition(rule.ConditionExpression, testData);
                    result.CompiledExpression = rule.ConditionExpression;
                }
                catch (Exception ex)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Invalid condition expression: {ex.Message}");
                }

                // Validate actions
                if (rule.Actions == null || !rule.Actions.Any())
                {
                    result.Warnings.Add("Rule has no actions defined");
                }

                // Validate dependencies
                if (rule.DependsOnRules != null && rule.DependsOnRules.Any())
                {
                    var isValidDeps = await _repository.ValidateRuleDependenciesAsync(rule.RuleId, rule.DependsOnRules);
                    if (!isValidDeps)
                    {
                        result.IsValid = false;
                        result.Errors.Add("Circular dependency detected");
                    }
                }

                return ApiResponse<RuleValidationResultDto>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating rule {Id}", id);
                return ApiResponse<RuleValidationResultDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ValidateRuleDependenciesAsync(string id)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return ApiResponse<bool>.Fail($"Rule with ID {id} not found");

                var isValid = await _repository.ValidateRuleDependenciesAsync(rule.RuleId, rule.DependsOnRules);
                return ApiResponse<bool>.Ok(isValid, isValid ? "Dependencies are valid" : "Circular dependency detected");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating rule dependencies for {Id}", id);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        #endregion

        #region Bulk Operations

        public async Task<ApiResponse<BulkRuleOperationResponseDto>> BulkActivateRulesAsync(BulkRuleOperationDto dto)
        {
            try
            {
                var response = new BulkRuleOperationResponseDto
                {
                    ProcessedAt = DateTime.UtcNow
                };

                foreach (var ruleId in dto.RuleIds)
                {
                    try
                    {
                        var rule = await _repository.GetByRuleIdAsync(ruleId);
                        if (rule == null)
                        {
                            response.Failed++;
                            response.Errors.Add(new BulkRuleOperationErrorDto
                            {
                                RuleId = ruleId,
                                ErrorCode = "NOT_FOUND",
                                ErrorMessage = $"Rule {ruleId} not found"
                            });
                            continue;
                        }

                        rule.Activate(dto.PerformedBy);
                        await _repository.UpdateAsync(rule);
                        response.Successful++;
                    }
                    catch (Exception ex)
                    {
                        response.Failed++;
                        response.Errors.Add(new BulkRuleOperationErrorDto
                        {
                            RuleId = ruleId,
                            ErrorCode = "ACTIVATION_FAILED",
                            ErrorMessage = ex.Message
                        });
                    }
                }

                response.TotalProcessed = dto.RuleIds.Count;
                return ApiResponse<BulkRuleOperationResponseDto>.Ok(response, $"Activated {response.Successful} of {response.TotalProcessed} rules");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk activate rules");
                return ApiResponse<BulkRuleOperationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<BulkRuleOperationResponseDto>> BulkDeactivateRulesAsync(BulkRuleOperationDto dto)
        {
            try
            {
                var response = new BulkRuleOperationResponseDto
                {
                    ProcessedAt = DateTime.UtcNow
                };

                foreach (var ruleId in dto.RuleIds)
                {
                    try
                    {
                        var rule = await _repository.GetByRuleIdAsync(ruleId);
                        if (rule == null)
                        {
                            response.Failed++;
                            response.Errors.Add(new BulkRuleOperationErrorDto
                            {
                                RuleId = ruleId,
                                ErrorCode = "NOT_FOUND",
                                ErrorMessage = $"Rule {ruleId} not found"
                            });
                            continue;
                        }

                        rule.Deactivate(dto.PerformedBy, dto.Reason ?? "Bulk deactivation");
                        await _repository.UpdateAsync(rule);
                        response.Successful++;
                    }
                    catch (Exception ex)
                    {
                        response.Failed++;
                        response.Errors.Add(new BulkRuleOperationErrorDto
                        {
                            RuleId = ruleId,
                            ErrorCode = "DEACTIVATION_FAILED",
                            ErrorMessage = ex.Message
                        });
                    }
                }

                response.TotalProcessed = dto.RuleIds.Count;
                return ApiResponse<BulkRuleOperationResponseDto>.Ok(response, $"Deactivated {response.Successful} of {response.TotalProcessed} rules");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk deactivate rules");
                return ApiResponse<BulkRuleOperationResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Private Helper Methods

        private bool EvaluateCondition(string conditionExpression, Dictionary<string, object> inputData)
        {
            var expression = conditionExpression;

            // Replace placeholders with actual values
            foreach (var kvp in inputData)
            {
                var placeholder = $"{{{{{kvp.Key}}}}}";
                var value = kvp.Value?.ToString() ?? "null";
                expression = expression.Replace(placeholder, value);
            }

            try
            {
                // Handle AND conditions
                if (expression.Contains(" AND "))
                {
                    var parts = expression.Split(" AND ");
                    return parts.All(part => EvaluateSimpleCondition(part.Trim()));
                }

                // Handle OR conditions
                if (expression.Contains(" OR "))
                {
                    var parts = expression.Split(" OR ");
                    return parts.Any(part => EvaluateSimpleCondition(part.Trim()));
                }

                return EvaluateSimpleCondition(expression);
            }
            catch
            {
                return false;
            }
        }

        private bool EvaluateSimpleCondition(string expression)
        {
            if (expression.Contains(">="))
            {
                var parts = expression.Split(">=");
                if (parts.Length == 2)
                {
                    var left = Convert.ToDecimal(parts[0].Trim());
                    var right = Convert.ToDecimal(parts[1].Trim());
                    return left >= right;
                }
            }
            else if (expression.Contains("<="))
            {
                var parts = expression.Split("<=");
                if (parts.Length == 2)
                {
                    var left = Convert.ToDecimal(parts[0].Trim());
                    var right = Convert.ToDecimal(parts[1].Trim());
                    return left <= right;
                }
            }
            else if (expression.Contains(">"))
            {
                var parts = expression.Split('>');
                if (parts.Length == 2)
                {
                    var left = Convert.ToDecimal(parts[0].Trim());
                    var right = Convert.ToDecimal(parts[1].Trim());
                    return left > right;
                }
            }
            else if (expression.Contains("<"))
            {
                var parts = expression.Split('<');
                if (parts.Length == 2)
                {
                    var left = Convert.ToDecimal(parts[0].Trim());
                    var right = Convert.ToDecimal(parts[1].Trim());
                    return left < right;
                }
            }
            else if (expression.Contains("=="))
            {
                var parts = expression.Split("==");
                if (parts.Length == 2)
                {
                    return parts[0].Trim() == parts[1].Trim();
                }
            }
            else if (expression.Contains("!="))
            {
                var parts = expression.Split("!=");
                if (parts.Length == 2)
                {
                    return parts[0].Trim() != parts[1].Trim();
                }
            }

            return false;
        }

        #endregion
    }
}