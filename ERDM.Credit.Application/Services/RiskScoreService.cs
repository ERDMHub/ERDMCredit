using AutoMapper;
using ERDM.Credit.Contracts.DTOs.RiskScoreDtos;
using ERDM.Credit.Contracts.Wrapper;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;
using ERDM.Credit.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ERDM.Credit.Application.Services
{
    public class RiskScoreService : IRiskScoreService
    {
        private readonly IRiskScoreRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RiskScoreService> _logger;

        public RiskScoreService(
            IRiskScoreRepository repository,
            IMapper mapper,
            ILogger<RiskScoreService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Create Operations

        public async Task<ApiResponse<RiskScoreResponseDto>> CreateAsync(CreateRiskScoreDto dto)
        {
            try
            {
                _logger.LogInformation("Creating risk score for customer {CustomerId}", dto.CustomerId);

                var riskScore = _mapper.Map<RiskScore>(dto);
                
                // Calculate risk metrics if not provided
                if (!dto.ProbabilityOfDefault.HasValue)
                {
                    riskScore.UpdateRiskMetrics(
                        CalculateProbabilityOfDefault(dto.ScoreValue),
                        CalculateLossGivenDefault(dto.RiskCategory),
                        CalculateExposureAtDefault(dto.ScoreValue)
                    );
                }

                // Raise domain event
                riskScore.RaiseCreatedEvent();
                
                var result = await _repository.AddAsync(riskScore);

                // Check for threshold breach
                if (dto.ScoreValue < 600)
                {
                    _logger.LogWarning("Risk threshold breached for customer {CustomerId}. Score: {Score}", 
                        dto.CustomerId, dto.ScoreValue);
                }

                var response = _mapper.Map<RiskScoreResponseDto>(result);
                return ApiResponse<RiskScoreResponseDto>.Ok(response, "Risk score created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating risk score for customer {CustomerId}", dto.CustomerId);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Read Operations

        public async Task<ApiResponse<RiskScoreResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var riskScore = await _repository.GetByIdAsync(id);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score with ID {id} not found");

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk score {Id}", id);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RiskScoreResponseDto>> GetByRiskScoreIdAsync(string riskScoreId)
        {
            try
            {
                var riskScore = await _repository.GetByRiskScoreIdAsync(riskScoreId);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score {riskScoreId} not found");

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk score {RiskScoreId}", riskScoreId);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RiskScoreResponseDto>>> GetByCustomerIdAsync(string customerId)
        {
            try
            {
                var riskScores = await _repository.GetByCustomerIdAsync(customerId);
                var response = _mapper.Map<List<RiskScoreResponseDto>>(riskScores);
                return ApiResponse<List<RiskScoreResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk scores for customer {CustomerId}", customerId);
                return ApiResponse<List<RiskScoreResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RiskScoreResponseDto>> GetLatestByCustomerIdAsync(string customerId)
        {
            try
            {
                var riskScore = await _repository.GetLatestByCustomerIdAsync(customerId);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"No risk score found for customer {customerId}");

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest risk score for customer {CustomerId}", customerId);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RiskScoreResponseDto>>> GetByApplicationIdAsync(string applicationId)
        {
            try
            {
                var riskScores = await _repository.GetByApplicationIdAsync(applicationId);
                var response = _mapper.Map<List<RiskScoreResponseDto>>(riskScores);
                return ApiResponse<List<RiskScoreResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk scores for application {ApplicationId}", applicationId);
                return ApiResponse<List<RiskScoreResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RiskScoreResponseDto>>> GetByAccountIdAsync(string accountId)
        {
            try
            {
                var riskScores = await _repository.GetByAccountIdAsync(accountId);
                var response = _mapper.Map<List<RiskScoreResponseDto>>(riskScores);
                return ApiResponse<List<RiskScoreResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk scores for account {AccountId}", accountId);
                return ApiResponse<List<RiskScoreResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<RiskScoreResponseDto>>> GetAllAsync(RiskScoreQueryDto query)
        {
            try
            {
                var filter = new RiskScoreFilter
                {
                    CustomerId = query.CustomerId,
                    ApplicationId = query.ApplicationId,
                    AccountId = query.AccountId,
                    ScoreType = query.ScoreType != null ? Enum.Parse<ScoreType>(query.ScoreType) : null,
                    RiskCategory = query.RiskCategory != null ? Enum.Parse<RiskCategory>(query.RiskCategory) : null,
                    MinScore = query.MinScore,
                    MaxScore = query.MaxScore,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate,
                    IsValid = query.IsValid
                };

                var result = await _repository.GetPaginatedWithFiltersAsync(
                    filter,
                    query.PageNumber,
                    query.PageSize,
                    query.SortBy,
                    query.SortDescending);

                var response = new PaginatedResponse<RiskScoreResponseDto>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages,
                    HasPrevious = result.HasPrevious,
                    HasNext = result.HasNext,
                    Data = _mapper.Map<IEnumerable<RiskScoreResponseDto>>(result.Data)
                };

                return ApiResponse<PaginatedResponse<RiskScoreResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk scores");
                return ApiResponse<PaginatedResponse<RiskScoreResponseDto>>.Fail(ex.Message);
            }
        }

        #endregion

        #region Update Operations

        public async Task<ApiResponse<RiskScoreResponseDto>> UpdateScoreAsync(string id, UpdateRiskScoreDto dto)
        {
            try
            {
                var riskScore = await _repository.GetByIdAsync(id);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score with ID {id} not found");

                // Call entity method to raise event
                riskScore.UpdateScore(dto.ScoreValue, dto.ScoreGrade, 
                    Enum.Parse<RiskCategory>(dto.RiskCategory), dto.UpdatedBy);
                
                await _repository.UpdateAsync(riskScore);

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response, "Risk score updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating risk score {Id}", id);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RiskScoreResponseDto>> UpdateRiskMetricsAsync(string id, UpdateRiskMetricsDto dto)
        {
            try
            {
                var riskScore = await _repository.GetByIdAsync(id);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score with ID {id} not found");

                // Call entity method to raise event
                riskScore.UpdateRiskMetrics(dto.ProbabilityOfDefault, dto.LossGivenDefault, dto.ExposureAtDefault);
                
                await _repository.UpdateAsync(riskScore);

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response, "Risk metrics updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating risk metrics for {Id}", id);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RiskScoreResponseDto>> AddRiskFactorAsync(string id, AddRiskFactorDto dto)
        {
            try
            {
                var riskScore = await _repository.GetByIdAsync(id);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score with ID {id} not found");

                // Call entity method to raise event
                riskScore.AddRiskFactor(dto.FactorName, dto.FactorValue, dto.Weight, dto.Impact);
                
                await _repository.UpdateAsync(riskScore);

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response, "Risk factor added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding risk factor for {Id}", id);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RiskScoreResponseDto>> InvalidateScoreAsync(string id, InvalidateRiskScoreDto dto)
        {
            try
            {
                var riskScore = await _repository.GetByIdAsync(id);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score with ID {id} not found");

                // Call entity method to raise event
                riskScore.InvalidateScore(dto.InvalidatedBy, dto.Reason);
                
                await _repository.UpdateAsync(riskScore);

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response, "Risk score invalidated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating risk score {Id}", id);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RiskScoreResponseDto>> ScheduleReviewAsync(string id, ScheduleReviewDto dto)
        {
            try
            {
                var riskScore = await _repository.GetByIdAsync(id);
                if (riskScore == null)
                    return ApiResponse<RiskScoreResponseDto>.Fail($"Risk score with ID {id} not found");

                // Call entity method to raise event
                riskScore.ScheduleReview(dto.ReviewDate, dto.ScheduledBy);
                
                await _repository.UpdateAsync(riskScore);

                var response = _mapper.Map<RiskScoreResponseDto>(riskScore);
                return ApiResponse<RiskScoreResponseDto>.Ok(response, "Review scheduled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling review for risk score {Id}", id);
                return ApiResponse<RiskScoreResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Statistics and Reports

        public async Task<ApiResponse<RiskScoreStatisticsDto>> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var riskScores = await _repository.GetAllAsync();
                var filteredScores = riskScores.AsEnumerable();

                if (fromDate.HasValue)
                    filteredScores = filteredScores.Where(s => s.ScoringDate >= fromDate.Value);
                if (toDate.HasValue)
                    filteredScores = filteredScores.Where(s => s.ScoringDate <= toDate.Value);

                var scoreList = filteredScores.ToList();
                var validScores = scoreList.Where(s => s.IsValid && (!s.ValidUntil.HasValue || s.ValidUntil > DateTime.UtcNow)).ToList();

                var response = new RiskScoreStatisticsDto
                {
                    TotalScores = scoreList.Count,
                    ActiveScores = validScores.Count,
                    ExpiredScores = scoreList.Count(s => !s.IsValid),
                    ScoresByCategory = scoreList
                        .GroupBy(s => s.RiskCategory.ToString())
                        .ToDictionary(g => g.Key, g => g.Count()),
                    ScoresByType = scoreList
                        .GroupBy(s => s.ScoreType.ToString())
                        .ToDictionary(g => g.Key, g => g.Count()),
                    AverageScore = validScores.Any() ? (int)validScores.Average(s => s.ScoreValue) : 0,
                    MinScore = validScores.Any() ? validScores.Min(s => s.ScoreValue) : 0,
                    MaxScore = validScores.Any() ? validScores.Max(s => s.ScoreValue) : 0,
                    AverageProbabilityOfDefault = validScores.Any() ? validScores.Average(s => s.ProbabilityOfDefault) : 0,
                    TotalExpectedLoss = validScores.Sum(s => s.ExpectedLoss),
                    ScoreDistribution = GetScoreDistribution(validScores),
                    ScoreTrend = GetScoreTrend(scoreList.OrderBy(s => s.ScoringDate).Take(30).ToList()),
                    AsOfDate = DateTime.UtcNow
                };

                return ApiResponse<RiskScoreStatisticsDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting risk score statistics");
                return ApiResponse<RiskScoreStatisticsDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<CustomerRiskProfileDto>> GetCustomerRiskProfileAsync(string customerId)
        {
            try
            {
                var riskScores = await _repository.GetByCustomerIdAsync(customerId);
                var scoreList = riskScores.ToList();
                var currentScore = scoreList.OrderByDescending(s => s.ScoringDate).FirstOrDefault();

                var response = new CustomerRiskProfileDto
                {
                    CustomerId = customerId,
                    CurrentRiskScore = currentScore != null ? _mapper.Map<RiskScoreResponseDto>(currentScore) : null,
                    ScoreHistory = _mapper.Map<List<RiskScoreResponseDto>>(scoreList.OrderByDescending(s => s.ScoringDate).Take(10)),
                    OverallRiskCategory = currentScore?.RiskCategory ?? RiskCategory.Medium,
                    ProbabilityOfDefault = currentScore?.ProbabilityOfDefault ?? 0,
                    ExpectedLoss = currentScore?.ExpectedLoss ?? 0,
                    KeyRiskFactors = currentScore?.RiskFactors.Select(rf => new RiskFactorDto
                    {
                        FactorId = rf.FactorId,
                        FactorName = rf.FactorName,
                        FactorValue = rf.FactorValue,
                        Weight = rf.Weight,
                        Impact = rf.Impact,
                        AssessedAt = rf.AssessedAt
                    }).ToList() ?? new List<RiskFactorDto>(),
                    LastAssessmentDate = currentScore?.ScoringDate ?? DateTime.UtcNow,
                    NextReviewDate = currentScore?.NextReviewDate,
                    Recommendations = GenerateRecommendations(currentScore)
                };

                return ApiResponse<CustomerRiskProfileDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer risk profile for {CustomerId}", customerId);
                return ApiResponse<CustomerRiskProfileDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<RiskThresholdAlertDto>>> GetRiskThresholdAlertsAsync(int threshold = 600)
        {
            try
            {
                var validScores = await _repository.GetValidScoresAsync();
                var alerts = validScores
                    .Where(s => s.ScoreValue < threshold)
                    .Select(s => new RiskThresholdAlertDto
                    {
                        CustomerId = s.CustomerId,
                        AccountId = s.AccountId,
                        CurrentScore = s.ScoreValue,
                        ThresholdValue = threshold,
                        ThresholdType = "Low Score",
                        RiskCategory = s.RiskCategory,
                        AlertDate = DateTime.UtcNow,
                        RecommendedAction = GetRecommendedAction(s.RiskCategory)
                    }).ToList();

                return ApiResponse<List<RiskThresholdAlertDto>>.Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting risk threshold alerts");
                return ApiResponse<List<RiskThresholdAlertDto>>.Fail(ex.Message);
            }
        }

        #endregion

        #region Bulk Operations

        public async Task<ApiResponse<int>> BulkUpdateValidityAsync()
        {
            try
            {
                var count = await _repository.BulkUpdateValidityAsync(DateTime.UtcNow);
                return ApiResponse<int>.Ok(count, $"Updated validity for {count} expired scores");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating validity");
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        #endregion

        #region Private Helper Methods

        private decimal CalculateProbabilityOfDefault(int scoreValue)
        {
            // Simple PD calculation based on score
            if (scoreValue >= 800) return 0.01m;
            if (scoreValue >= 700) return 0.02m;
            if (scoreValue >= 600) return 0.05m;
            if (scoreValue >= 500) return 0.10m;
            if (scoreValue >= 400) return 0.20m;
            return 0.35m;
        }

        private decimal CalculateLossGivenDefault(string riskCategory)
        {
            return riskCategory switch
            {
                "VeryLow" => 0.10m,
                "Low" => 0.20m,
                "Medium" => 0.35m,
                "High" => 0.50m,
                "VeryHigh" => 0.70m,
                _ => 0.35m
            };
        }

        private decimal CalculateExposureAtDefault(int scoreValue)
        {
            // Simplified EAD calculation
            if (scoreValue >= 700) return 0.5m;
            if (scoreValue >= 600) return 0.7m;
            if (scoreValue >= 500) return 0.85m;
            return 1.0m;
        }

        private Dictionary<string, int> GetScoreDistribution(List<RiskScore> scores)
        {
            var distribution = new Dictionary<string, int>();
            var ranges = new[] { "0-300", "301-400", "401-500", "501-600", "601-700", "701-800", "801-900", "901-1000" };
            
            foreach (var range in ranges)
            {
                var parts = range.Split('-');
                var min = int.Parse(parts[0]);
                var max = int.Parse(parts[1]);
                distribution[range] = scores.Count(s => s.ScoreValue >= min && s.ScoreValue <= max);
            }
            
            return distribution;
        }

        private List<RiskTrendDto> GetScoreTrend(List<RiskScore> scores)
        {
            return scores
                .GroupBy(s => s.ScoringDate.Date)
                .Select(g => new RiskTrendDto
                {
                    Date = g.Key,
                    AverageScore = (int)g.Average(s => s.ScoreValue),
                    ScoreCount = g.Count(),
                    AverageProbabilityOfDefault = g.Average(s => s.ProbabilityOfDefault)
                })
                .OrderBy(t => t.Date)
                .ToList();
        }

        private string GenerateRecommendations(RiskScore? riskScore)
        {
            if (riskScore == null) return "No risk assessment available.";
            
            return riskScore.RiskCategory switch
            {
                RiskCategory.VeryLow => "Continue monitoring. Consider credit limit increase.",
                RiskCategory.Low => "Standard monitoring. Review annually.",
                RiskCategory.Medium => "Regular monitoring required. Consider reduced exposure.",
                RiskCategory.High => "Close monitoring. Consider credit limit decrease.",
                RiskCategory.VeryHigh => "Immediate review required. Consider account suspension.",
                _ => "Standard monitoring recommended."
            };
        }

        private string GetRecommendedAction(RiskCategory riskCategory)
        {
            return riskCategory switch
            {
                RiskCategory.High => "Schedule immediate account review",
                RiskCategory.VeryHigh => "Suspend account pending review",
                _ => "Monitor account activity"
            };
        }

        #endregion
    }
}