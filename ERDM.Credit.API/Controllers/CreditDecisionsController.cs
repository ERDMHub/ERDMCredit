using ERDM.Credit.Application.Services;
using ERDM.Credit.Contracts.DTOs.CreditDecisionDtos;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Credit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditDecisionsController : ControllerBase
    {
        private readonly ICreditDecisionService _service;
        private readonly ILogger<CreditDecisionsController> _logger;

        public CreditDecisionsController(
            ICreditDecisionService service,
            ILogger<CreditDecisionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCreditDecisionDto dto)
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

        [HttpGet("decision-id/{decisionId}")]
        public async Task<IActionResult> GetByDecisionId(string decisionId)
        {
            var result = await _service.GetByDecisionIdAsync(decisionId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetByApplicationId(string applicationId)
        {
            var result = await _service.GetByApplicationIdAsync(applicationId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var result = await _service.GetByCustomerIdAsync(customerId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CreditDecisionQueryDto query)
        {
            var result = await _service.GetAllAsync(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveDecision(string id, [FromBody] ApproveCreditDecisionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ApproveDecisionAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/decline")]
        public async Task<IActionResult> DeclineDecision(string id, [FromBody] DeclineCreditDecisionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeclineDecisionAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/accept-counter-offer")]
        public async Task<IActionResult> AcceptCounterOffer(string id, [FromBody] AcceptCounterOfferDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AcceptCounterOfferAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromQuery] string status, [FromQuery] string updatedBy)
        {
            var result = await _service.UpdateDecisionStatusAsync(id, status, updatedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/conditions")]
        public async Task<IActionResult> UpdateConditions(string id, [FromBody] List<UpdateUnderwritingConditionDto> conditions)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateConditionsAsync(id, conditions);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{decisionId}/conditions/{conditionId}/met")]
        public async Task<IActionResult> MetCondition(string decisionId, string conditionId, [FromQuery] string metBy)
        {
            var result = await _service.MetConditionAsync(decisionId, conditionId, metBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _service.GetStatisticsAsync(fromDate, toDate);
            return Ok(result);
        }
    }
}