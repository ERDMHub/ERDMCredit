using ERDM.Credit.Application.Services;
using ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Credit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnderwritingRulesController : ControllerBase
    {
        private readonly IUnderwritingRuleService _service;
        private readonly ILogger<UnderwritingRulesController> _logger;

        public UnderwritingRulesController(
            IUnderwritingRuleService service,
            ILogger<UnderwritingRulesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region Create Operations

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUnderwritingRuleDto dto)
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

        [HttpGet("rule-id/{ruleId}")]
        public async Task<IActionResult> GetByRuleId(string ruleId)
        {
            var result = await _service.GetByRuleIdAsync(ruleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("rule-code/{ruleCode}")]
        public async Task<IActionResult> GetByRuleCode(string ruleCode)
        {
            var result = await _service.GetByRuleCodeAsync(ruleCode);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UnderwritingRuleQueryDto query)
        {
            var result = await _service.GetAllAsync(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRules()
        {
            var result = await _service.GetActiveRulesAsync();
            return Ok(result);
        }

        [HttpGet("type/{ruleType}")]
        public async Task<IActionResult> GetByType(string ruleType)
        {
            var result = await _service.GetRulesByTypeAsync(ruleType);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var result = await _service.GetRulesByCategoryAsync(category);
            return result.Success ? Ok(result) : NotFound(result);
        }

        #endregion

        #region Update Operations

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRule(string id, [FromBody] UpdateUnderwritingRuleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateRuleAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateRule(string id, [FromQuery] string activatedBy)
        {
            var result = await _service.ActivateRuleAsync(id, activatedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateRule(string id, [FromQuery] string deactivatedBy, [FromQuery] string reason)
        {
            var result = await _service.DeactivateRuleAsync(id, deactivatedBy, reason);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveRule(string id, [FromQuery] string approvedBy)
        {
            var result = await _service.ApproveRuleAsync(id, approvedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectRule(string id, [FromQuery] string rejectedBy, [FromQuery] string reason)
        {
            var result = await _service.RejectRuleAsync(id, rejectedBy, reason);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Execution Operations

        [HttpPost("{ruleId}/execute")]
        public async Task<IActionResult> ExecuteRule(string ruleId, [FromBody] ExecuteRuleRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ExecuteRuleAsync(ruleId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("execute-set")]
        public async Task<IActionResult> ExecuteRuleSet([FromBody] ExecuteRuleSetRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ExecuteRuleSetAsync(request.RuleIds, request.Request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Validation Operations

        [HttpGet("{id}/validate")]
        public async Task<IActionResult> ValidateRule(string id)
        {
            var result = await _service.ValidateRuleAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/validate-dependencies")]
        public async Task<IActionResult> ValidateDependencies(string id)
        {
            var result = await _service.ValidateRuleDependenciesAsync(id);
            return Ok(result);
        }

        #endregion

        #region Statistics and Reports

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _service.GetStatisticsAsync();
            return Ok(result);
        }

        [HttpGet("top-performing")]
        public async Task<IActionResult> GetTopPerformingRules([FromQuery] int count = 10)
        {
            var result = await _service.GetTopPerformingRulesAsync(count);
            return Ok(result);
        }

        [HttpGet("underperforming")]
        public async Task<IActionResult> GetUnderperformingRules([FromQuery] int count = 10)
        {
            var result = await _service.GetUnderperformingRulesAsync(count);
            return Ok(result);
        }

        #endregion

        #region Bulk Operations

        [HttpPost("bulk/activate")]
        public async Task<IActionResult> BulkActivateRules([FromBody] BulkRuleOperationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.BulkActivateRulesAsync(dto);
            return Ok(result);
        }

        [HttpPost("bulk/deactivate")]
        public async Task<IActionResult> BulkDeactivateRules([FromBody] BulkRuleOperationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.BulkDeactivateRulesAsync(dto);
            return Ok(result);
        }

        #endregion
    }

    public class ExecuteRuleSetRequestDto
    {
        public List<string> RuleIds { get; set; } = new();
        public ExecuteRuleRequestDto Request { get; set; } = new();
    }
}