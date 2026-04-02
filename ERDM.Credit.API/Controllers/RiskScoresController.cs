using ERDM.Credit.Application.Services;
using ERDM.Credit.Contracts.DTOs.RiskScoreDtos;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Credit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskScoresController : ControllerBase
    {
        private readonly IRiskScoreService _service;
        private readonly ILogger<RiskScoresController> _logger;

        public RiskScoresController(
            IRiskScoreService service,
            ILogger<RiskScoresController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region Create Operations

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRiskScoreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            return result.Success
                ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result)
                : BadRequest(result);
        }

        #endregion

        #region Read Operations

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("risk-score-id/{riskScoreId}")]
        public async Task<IActionResult> GetByRiskScoreId(string riskScoreId)
        {
            var result = await _service.GetByRiskScoreIdAsync(riskScoreId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var result = await _service.GetByCustomerIdAsync(customerId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}/latest")]
        public async Task<IActionResult> GetLatestByCustomerId(string customerId)
        {
            var result = await _service.GetLatestByCustomerIdAsync(customerId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetByApplicationId(string applicationId)
        {
            var result = await _service.GetByApplicationIdAsync(applicationId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccountId(string accountId)
        {
            var result = await _service.GetByAccountIdAsync(accountId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RiskScoreQueryDto query)
        {
            var result = await _service.GetAllAsync(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Update Operations

        [HttpPut("{id}/score")]
        public async Task<IActionResult> UpdateScore(string id, [FromBody] UpdateRiskScoreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateScoreAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/risk-metrics")]
        public async Task<IActionResult> UpdateRiskMetrics(string id, [FromBody] UpdateRiskMetricsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateRiskMetricsAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/risk-factors")]
        public async Task<IActionResult> AddRiskFactor(string id, [FromBody] AddRiskFactorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddRiskFactorAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/invalidate")]
        public async Task<IActionResult> InvalidateScore(string id, [FromBody] InvalidateRiskScoreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.InvalidateScoreAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/schedule-review")]
        public async Task<IActionResult> ScheduleReview(string id, [FromBody] ScheduleReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ScheduleReviewAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Statistics and Reports

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _service.GetStatisticsAsync(fromDate, toDate);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}/profile")]
        public async Task<IActionResult> GetCustomerRiskProfile(string customerId)
        {
            var result = await _service.GetCustomerRiskProfileAsync(customerId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("threshold-alerts")]
        public async Task<IActionResult> GetRiskThresholdAlerts([FromQuery] int threshold = 600)
        {
            var result = await _service.GetRiskThresholdAlertsAsync(threshold);
            return Ok(result);
        }

        #endregion

        #region Bulk Operations

        [HttpPost("bulk/update-validity")]
        public async Task<IActionResult> BulkUpdateValidity()
        {
            var result = await _service.BulkUpdateValidityAsync();
            return Ok(result);
        }

        #endregion
    }
}