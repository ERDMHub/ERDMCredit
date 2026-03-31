using ERDM.Credit.Application.Services;
using ERDM.Credit.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Credit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditApplicationsController : ControllerBase
    {
        private readonly ICreditApplicationService _service;
        private readonly ILogger<CreditApplicationsController> _logger;

        public CreditApplicationsController(
            ICreditApplicationService service,
            ILogger<CreditApplicationsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCreditApplicationDto dto)
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

        [HttpGet("number/{applicationNumber}")]
        public async Task<IActionResult> GetByNumber(string applicationNumber)
        {
            var result = await _service.GetByApplicationNumberAsync(applicationNumber);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ApplicationQueryDto query)
        {
            var result = await _service.GetAllAsync(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(string id)
        {
            var result = await _service.SubmitAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(string id, [FromBody] ApproveApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ApproveAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/decline")]
        public async Task<IActionResult> Decline(string id, [FromBody] DeclineApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeclineAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _service.GetStatisticsAsync();
            return Ok(result);
        }
    }
}