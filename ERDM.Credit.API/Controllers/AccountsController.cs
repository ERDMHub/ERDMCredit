using ERDM.Credit.Application.Services;
using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Credit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            IAccountService service,
            ILogger<AccountsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region Create Operations

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Creating new account for application: {ApplicationId}", dto.ApplicationId);
            var result = await _service.CreateAsync(dto);

            return result.Success
                ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result)
                : BadRequest(result);
        }

        [HttpPost("from-application/{applicationId}")]
        public async Task<IActionResult> CreateFromApplication(
            string applicationId,
            [FromBody] CreateAccountFromApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Creating account from application: {ApplicationId}", applicationId);
            var result = await _service.CreateFromApplicationAsync(applicationId, dto);

            return result.Success
                ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result)
                : BadRequest(result);
        }

        #endregion

        #region Read Operations

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogDebug("Getting account by id: {AccountId}", id);
            var result = await _service.GetByIdAsync(id);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("number/{accountNumber}")]
        public async Task<IActionResult> GetByAccountNumber(string accountNumber)
        {
            _logger.LogDebug("Getting account by number: {AccountNumber}", accountNumber);
            var result = await _service.GetByAccountNumberAsync(accountNumber);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetByApplicationId(string applicationId)
        {
            _logger.LogDebug("Getting account by application id: {ApplicationId}", applicationId);
            var result = await _service.GetByApplicationIdAsync(applicationId);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            _logger.LogDebug("Getting accounts for customer: {CustomerId}", customerId);
            var result = await _service.GetByCustomerIdAsync(customerId);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AccountQueryDto query)
        {
            _logger.LogInformation("Getting all accounts with filters");
            var result = await _service.GetAllAsync(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Status Operations

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateAccount(string id, [FromBody] ActivateAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Activating account: {AccountId}", id);
            var result = await _service.ActivateAccountAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseAccount(string id, [FromBody] CloseAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Closing account: {AccountId}", id);
            var result = await _service.CloseAccountAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/suspend")]
        public async Task<IActionResult> SuspendAccount(string id, [FromBody] SuspendAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Suspending account: {AccountId}", id);
            var result = await _service.SuspendAccountAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/restructure")]
        public async Task<IActionResult> RestructureAccount(string id, [FromBody] RestructureAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Restructuring account: {AccountId}", id);
            var result = await _service.RestructureAccountAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/mark-delinquent")]
        public async Task<IActionResult> MarkAsDelinquent(string id, [FromBody] MarkDelinquentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogWarning("Marking account as delinquent: {AccountId}, Days Overdue: {DaysOverdue}", id, dto.DaysOverdue);
            var result = await _service.MarkAsDelinquentAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/write-off")]
        public async Task<IActionResult> WriteOffAccount(string id, [FromBody] WriteOffAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogWarning("Writing off account: {AccountId}, Amount: {WriteOffAmount}", id, dto.WriteOffAmount);
            var result = await _service.WriteOffAccountAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Payment Operations

        [HttpPost("{accountId}/payments")]
        public async Task<IActionResult> ProcessPayment(string accountId, [FromBody] ProcessPaymentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Processing payment for account: {AccountId}, Amount: {Amount}", accountId, dto.Amount);
            var result = await _service.ProcessPaymentAsync(accountId, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{accountId}/payment-schedule")]
        public async Task<IActionResult> GetPaymentSchedule(string accountId)
        {
            _logger.LogDebug("Getting payment schedule for account: {AccountId}", accountId);
            var result = await _service.GetPaymentScheduleAsync(accountId);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("{accountId}/payment-history")]
        public async Task<IActionResult> GetPaymentHistory(
            string accountId,
            [FromQuery] PaymentHistoryQueryDto query)
        {
            _logger.LogDebug("Getting payment history for account: {AccountId}", accountId);
            var result = await _service.GetPaymentHistoryAsync(accountId, query);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("{accountId}/payments/{paymentId}/reverse")]
        public async Task<IActionResult> ReversePayment(
            string accountId,
            string paymentId,
            [FromBody] ReversePaymentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogWarning("Reversing payment for account: {AccountId}, Payment: {PaymentId}", accountId, paymentId);
            var result = await _service.ReversePaymentAsync(accountId, paymentId, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Financial Operations

        [HttpPatch("{id}/outstanding-balance")]
        public async Task<IActionResult> UpdateOutstandingBalance(
            string id,
            [FromBody] UpdateBalanceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Updating outstanding balance for account: {AccountId}, New Balance: {NewBalance}", id, dto.NewOutstandingBalance);
            var result = await _service.UpdateOutstandingBalanceAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/available-credit")]
        public async Task<IActionResult> AdjustAvailableCredit(
            string id,
            [FromBody] AdjustCreditDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Adjusting available credit for account: {AccountId}, New Credit: {NewCredit}", id, dto.NewAvailableCredit);
            var result = await _service.AdjustAvailableCreditAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/late-fee")]
        public async Task<IActionResult> ApplyLateFee(string id, [FromBody] ApplyLateFeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Applying late fee for account: {AccountId}, Amount: {LateFeeAmount}", id, dto.LateFeeAmount);
            var result = await _service.ApplyLateFeeAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/restructure-payment")]
        public async Task<IActionResult> RestructurePayment(
            string id,
            [FromBody] RestructurePaymentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Restructuring payment for account: {AccountId}, Type: {RestructuringType}", id, dto.RestructuringType);
            var result = await _service.RestructurePaymentAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Disbursement Operations

        [HttpPost("{id}/disburse")]
        public async Task<IActionResult> DisburseAmount(string id, [FromBody] DisburseAmountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Disbursing amount for account: {AccountId}, Amount: {Amount}", id, dto.Amount);
            var result = await _service.DisburseAmountAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/disbursement/confirm")]
        public async Task<IActionResult> ConfirmDisbursement(
            string id,
            [FromBody] ConfirmDisbursementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Confirming disbursement for account: {AccountId}, Reference: {TransactionReference}", id, dto.TransactionReference);
            var result = await _service.ConfirmDisbursementAsync(id, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Statistics and Reports

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] AccountStatisticsQueryDto query)
        {
            _logger.LogInformation("Getting account statistics");
            var result = await _service.GetStatisticsAsync(query);

            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetAccountSummary([FromQuery] string? customerId = null)
        {
            _logger.LogInformation("Getting account summary for customer: {CustomerId}", customerId ?? "All");
            var result = await _service.GetAccountSummaryAsync(customerId);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("portfolio-performance")]
        public async Task<IActionResult> GetPortfolioPerformance([FromQuery] PerformanceQueryDto query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Getting portfolio performance from {FromDate} to {ToDate}", query.FromDate, query.ToDate);
            var result = await _service.GetPortfolioPerformanceAsync(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Bulk Operations

        [HttpPost("bulk/status")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] BulkStatusUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Bulk updating status for {Count} accounts to {NewStatus}", dto.AccountIds.Count, dto.NewStatus);
            var result = await _service.BulkUpdateStatusAsync(dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("bulk/mark-overdue")]
        public async Task<IActionResult> BulkMarkOverdueAccounts([FromBody] BulkOverdueDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Bulk marking overdue accounts as of {AsOfDate}", dto.AsOfDate);
            var result = await _service.BulkMarkOverdueAccountsAsync(dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Validation and Utilities

        [HttpGet("{accountId}/validate-disbursement")]
        public async Task<IActionResult> ValidateAccountForDisbursement(string accountId)
        {
            _logger.LogDebug("Validating account for disbursement: {AccountId}", accountId);
            var result = await _service.ValidateAccountForDisbursementAsync(accountId);

            return Ok(result);
        }

        [HttpPost("calculate-emi")]
        public async Task<IActionResult> CalculateEMI([FromBody] CalculateEMIDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogDebug("Calculating EMI for principal: {Principal}, Rate: {Rate}, Term: {Term}",
                dto.PrincipalAmount, dto.InterestRate, dto.TermMonths);
            var result = await _service.CalculateEMIAsync(dto);

            return Ok(result);
        }

        [HttpPost("calculate-amortization")]
        public async Task<IActionResult> CalculateAmortizationSchedule([FromBody] CalculateAmortizationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogDebug("Calculating amortization schedule for principal: {Principal}, Rate: {Rate}, Term: {Term}",
                dto.PrincipalAmount, dto.InterestRate, dto.TermMonths);
            var result = await _service.CalculateAmortizationScheduleAsync(dto);

            return Ok(result);
        }

        #endregion
    }
}