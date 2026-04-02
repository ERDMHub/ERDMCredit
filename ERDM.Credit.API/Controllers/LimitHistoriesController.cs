using ERDM.Credit.Application.Services;
using ERDM.Credit.Contracts.DTOs.LimitHistoryDtos;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Credit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LimitHistoriesController : ControllerBase
    {
        private readonly ILimitHistoryService _service;
        private readonly ILogger<LimitHistoriesController> _logger;

        public LimitHistoriesController(
            ILimitHistoryService service,
            ILogger<LimitHistoriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLimitHistoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            return result.Success
                ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result)
                : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("history-id/{limitHistoryId}")]
        public async Task<IActionResult> GetByLimitHistoryId(string limitHistoryId)
        {
            var result = await _service.GetByLimitHistoryIdAsync(limitHistoryId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var result = await _service.GetByCustomerIdAsync(customerId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccountId(string accountId)
        {
            var result = await _service.GetByAccountIdAsync(accountId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] LimitHistoryQueryDto query)
        {
            var result = await _service.GetAllAsync(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("account/{accountId}/latest")]
        public async Task<IActionResult> GetLatestLimitChange(string accountId)
        {
            var result = await _service.GetLatestLimitChangeAsync(accountId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("account/{accountId}/current-limit")]
        public async Task<IActionResult> GetCurrentLimit(string accountId)
        {
            var result = await _service.GetCurrentLimitAsync(accountId);
            return Ok(result);
        }

        [HttpPut("{id}/expiry-date")]
        public async Task<IActionResult> UpdateExpiryDate(string id, [FromBody] ExtendTemporaryLimitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateExpiryDateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/revert")]
        public async Task<IActionResult> RevertLimit(string id, [FromBody] RevertLimitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.RevertLimitAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _service.GetStatisticsAsync(fromDate, toDate);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}/summary")]
        public async Task<IActionResult> GetCustomerLimitSummary(string customerId)
        {
            var result = await _service.GetCustomerLimitSummaryAsync(customerId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("bulk/expire-temporary")]
        public async Task<IActionResult> BulkExpireTemporaryLimits()
        {
            var result = await _service.BulkExpireTemporaryLimitsAsync();
            return Ok(result);
        }
    }
}