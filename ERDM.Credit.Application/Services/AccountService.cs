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
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository repository,
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Create Operations

        public async Task<ApiResponse<AccountResponseDto>> CreateAsync(CreateAccountDto dto)
        {
            try
            {
                _logger.LogInformation("Creating account for application {ApplicationId}", dto.ApplicationId);

                // Check if account already exists for this application
                var existingAccount = await _repository.GetByApplicationIdAsync(dto.ApplicationId);
                if (existingAccount != null && existingAccount.Count() > 0)
                    return ApiResponse<AccountResponseDto>.Fail($"Account already exists for application {dto.ApplicationId}");

                var account = _mapper.Map<Account>(dto);
                var result = await _repository.AddAsync(account);

                var response = _mapper.Map<AccountResponseDto>(result);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account for application {ApplicationId}", dto.ApplicationId);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> CreateFromApplicationAsync(string applicationId, CreateAccountFromApplicationDto dto)
        {
            try
            {
                _logger.LogInformation("Creating account from application {ApplicationId}", applicationId);

                // Check if account already exists
                var existingAccount = await _repository.GetByApplicationIdAsync(applicationId);
                if (existingAccount != null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account already exists for application {applicationId}");

                var account = _mapper.Map<Account>(dto);
                account.ApplicationId = applicationId;

                var result = await _repository.AddAsync(account);

                var response = _mapper.Map<AccountResponseDto>(result);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account created from application successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account from application {ApplicationId}", applicationId);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Read Operations

        public async Task<ApiResponse<AccountResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> GetByAccountNumberAsync(string accountNumber)
        {
            try
            {
                var account = await _repository.GetByAccountNumberAsync(accountNumber);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account {accountNumber} not found");

                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account {AccountNumber}", accountNumber);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> GetByApplicationIdAsync(string applicationId)
        {
            try
            {
                var account = await _repository.GetByApplicationIdAsync(applicationId);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account for application {applicationId} not found");

                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account for application {ApplicationId}", applicationId);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<AccountResponseDto>>> GetByCustomerIdAsync(string customerId)
        {
            try
            {
                var accounts = await _repository.GetByCustomerIdAsync(customerId);
                var response = _mapper.Map<List<AccountResponseDto>>(accounts);
                return ApiResponse<List<AccountResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accounts for customer {CustomerId}", customerId);
                return ApiResponse<List<AccountResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<AccountResponseDto>>> GetAllAsync(AccountQueryDto query)
        {
            try
            {
                var filter = BuildFilter(query);

                Expression<Func<Account, object>> sortExpression = null;

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    sortExpression = query.SortBy.ToLower() switch
                    {
                        "accountnumber" => x => x.AccountNumber,
                        "customerid" => x => x.CustomerId,
                        "producttype" => x => x.ProductType,
                        "status" => x => x.Status,
                        "outstandingbalance" => x => x.OutstandingBalance,
                        "nextpaymentduedate" => x => x.NextPaymentDueDate,
                        "createdat" => x => x.CreatedAt,
                        "updatedat" => x => x.UpdatedAt,
                        _ => x => x.CreatedAt
                    };
                }

                var result = await _repository.GetPaginatedAsync(
                    query.PageNumber,
                    query.PageSize,
                    filter,
                    sortExpression,
                    query.SortDescending);

                var response = new PaginatedResponse<AccountResponseDto>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = result.TotalPages,
                    HasPrevious = result.HasPrevious,
                    HasNext = result.HasNext,
                    Data = _mapper.Map<IEnumerable<AccountResponseDto>>(result.Data)
                };

                return ApiResponse<PaginatedResponse<AccountResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accounts");
                return ApiResponse<PaginatedResponse<AccountResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResponse<AccountResponseDto>>> GetAllAsync()
        {
            try
            {
                var accounts = await _repository.GetAllAsync();

                var response = new PaginatedResponse<AccountResponseDto>
                {
                    PageNumber = 1,
                    PageSize = accounts.Count(),
                    TotalCount = accounts.Count(),
                    TotalPages = 1,
                    HasPrevious = false,
                    HasNext = false,
                    Data = _mapper.Map<IEnumerable<AccountResponseDto>>(accounts)
                };

                return ApiResponse<PaginatedResponse<AccountResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all accounts");
                return ApiResponse<PaginatedResponse<AccountResponseDto>>.Fail(ex.Message);
            }
        }

        #endregion

        #region Status Operations

        public async Task<ApiResponse<AccountResponseDto>> ActivateAccountAsync(string id, ActivateAccountDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                if (account.Status != AccountStatus.Approved && account.Status != AccountStatus.PendingApproval)
                    return ApiResponse<AccountResponseDto>.Fail($"Account cannot be activated from status {account.Status}");

                var success = await _repository.UpdateAccountStatusAsync(id, AccountStatus.Active, dto.ActivationReason, dto.ActivatedBy);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to activate account");

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account activated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> CloseAccountAsync(string id, CloseAccountDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                if (account.OutstandingBalance > 0 && dto.FinalBalance == null)
                    return ApiResponse<AccountResponseDto>.Fail("Account has outstanding balance. Please provide final balance or process remaining payments.");

                var success = await _repository.UpdateAccountStatusAsync(id, AccountStatus.Closed, dto.ClosureReason, dto.ClosedBy);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to close account");

                if (dto.FinalBalance.HasValue)
                    await _repository.UpdateOutstandingBalanceAsync(id, dto.FinalBalance.Value);

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account closed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> SuspendAccountAsync(string id, SuspendAccountDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var success = await _repository.UpdateAccountStatusAsync(id, AccountStatus.Suspended, dto.SuspensionReason, dto.SuspendedBy);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to suspend account");

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account suspended successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> RestructureAccountAsync(string id, RestructureAccountDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                // Apply restructuring changes
                if (dto.NewTermMonths.HasValue)
                {
                    account.TermMonths = dto.NewTermMonths.Value;
                    account.TermYears = dto.NewTermMonths.Value / 12;
                }

                if (dto.NewInterestRate.HasValue)
                    account.InterestRate = dto.NewInterestRate.Value;

                if (dto.NewEmiAmount.HasValue)
                    account.EmiAmount = dto.NewEmiAmount.Value;

                if (dto.NewNextPaymentDueDate.HasValue)
                    account.NextPaymentDueDate = dto.NewNextPaymentDueDate.Value;

                if (dto.PrincipalWriteOff.HasValue)
                    await _repository.UpdateOutstandingBalanceAsync(id, account.OutstandingBalance - dto.PrincipalWriteOff.Value);

                // Update status
                var success = await _repository.UpdateAccountStatusAsync(id, AccountStatus.Restructured, dto.RestructuringReason, dto.RestructuredBy);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to restructure account");

                // Update account with new terms
                await _repository.UpdateAsync(account);

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account restructured successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restructuring account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> MarkAsDelinquentAsync(string id, MarkDelinquentDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var success = await _repository.RecordLatePaymentAsync(id, dto.DaysOverdue, 0);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to mark account as delinquent");

                await _repository.UpdateAccountStatusAsync(id, AccountStatus.Delinquent, dto.DelinquencyReason ?? "Account became delinquent", dto.MarkedBy);

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account marked as delinquent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking account as delinquent {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> WriteOffAccountAsync(string id, WriteOffAccountDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var success = await _repository.UpdateAccountStatusAsync(id, AccountStatus.WrittenOff, dto.WriteOffReason, dto.WrittenOffBy);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to write off account");

                if (dto.WriteOffType == "Partial" && dto.WriteOffAmount > 0)
                {
                    await _repository.UpdateOutstandingBalanceAsync(id, account.OutstandingBalance - dto.WriteOffAmount);
                }
                else if (dto.WriteOffType == "Full")
                {
                    await _repository.UpdateOutstandingBalanceAsync(id, 0);
                }

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Account written off successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing off account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Payment Operations

        public async Task<ApiResponse<PaymentResponseDto>> ProcessPaymentAsync(string accountId, ProcessPaymentDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(accountId);
                if (account == null)
                    return ApiResponse<PaymentResponseDto>.Fail($"Account with ID {accountId} not found");

                if (account.Status != AccountStatus.Active && account.Status != AccountStatus.Disbursed && account.Status != AccountStatus.Delinquent)
                    return ApiResponse<PaymentResponseDto>.Fail($"Account {account.Status} cannot accept payments");

                // Calculate payment allocation
                var (principalPaid, interestPaid, feesPaid) = AllocatePayment(account, dto.Amount);

                var payment = new PaymentHistory
                {
                    PaymentId = Guid.NewGuid().ToString(),
                    PaymentDate = dto.PaymentDate,
                    AmountDue = account.EmiAmount,
                    AmountPaid = dto.Amount,
                    PrincipalPaid = principalPaid,
                    InterestPaid = interestPaid,
                    FeesPaid = feesPaid,
                    PaymentMethod = Enum.Parse<RepaymentMethod>(dto.PaymentMethod),
                    TransactionReference = dto.TransactionReference,
                    Status = PaymentStatus.Paid,
                    DueDate = account.NextPaymentDueDate ?? DateTime.UtcNow,
                    LateDays = account.NextPaymentDueDate.HasValue && dto.PaymentDate > account.NextPaymentDueDate.Value
                        ? (dto.PaymentDate - account.NextPaymentDueDate.Value).Days
                        : 0,
                    LateFeeCharged = 0,
                    Notes = dto.Notes,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                await _repository.AddPaymentHistoryAsync(accountId, payment);
                await _repository.UpdateOutstandingBalanceAsync(accountId, account.OutstandingBalance - (principalPaid + feesPaid));

                var response = _mapper.Map<PaymentResponseDto>(payment);
                response.RemainingBalance = account.OutstandingBalance - (principalPaid + feesPaid);
                response.IsFullPayment = response.RemainingBalance <= 0;
                response.IsLatePayment = payment.LateDays > 0;
                response.LateDays = payment.LateDays;

                return ApiResponse<PaymentResponseDto>.Ok(response, "Payment processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for account {AccountId}", accountId);
                return ApiResponse<PaymentResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaymentScheduleResponseDto>> GetPaymentScheduleAsync(string accountId)
        {
            try
            {
                var account = await _repository.GetAccountWithPaymentsAsync(accountId);
                if (account == null)
                    return ApiResponse<PaymentScheduleResponseDto>.Fail($"Account with ID {accountId} not found");

                var schedule = GeneratePaymentSchedule(account);

                var response = new PaymentScheduleResponseDto
                {
                    AccountId = account.AccountId,
                    AccountNumber = account.AccountNumber,
                    TotalPrincipal = account.PrincipalAmount,
                    TotalInterest = schedule.Sum(x => x.InterestAmount),
                    TotalAmount = account.PrincipalAmount + schedule.Sum(x => x.InterestAmount),
                    Installments = schedule
                };

                return ApiResponse<PaymentScheduleResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating payment schedule for account {AccountId}", accountId);
                return ApiResponse<PaymentScheduleResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaymentHistoryResponseDto>> GetPaymentHistoryAsync(string accountId, PaymentHistoryQueryDto query)
        {
            try
            {
                var payments = await _repository.GetPaymentHistoryAsync(accountId, query.FromDate, query.ToDate);
                var paymentList = payments.ToList();

                var paymentDtos = _mapper.Map<List<PaymentResponseDto>>(paymentList);

                var summary = new PaymentSummaryDto
                {
                    TotalPayments = paymentList.Count,
                    TotalAmountPaid = paymentList.Sum(p => p.AmountPaid),
                    TotalPrincipalPaid = paymentList.Sum(p => p.PrincipalPaid),
                    TotalInterestPaid = paymentList.Sum(p => p.InterestPaid),
                    TotalFeesPaid = paymentList.Sum(p => p.FeesPaid),
                    LatePayments = paymentList.Count(p => p.LateDays > 0),
                    OnTimePayments = paymentList.Count(p => p.LateDays == 0),
                    AveragePaymentAmount = paymentList.Any() ? paymentList.Average(p => p.AmountPaid) : 0,
                    LastPaymentDate = paymentList.OrderByDescending(p => p.PaymentDate).FirstOrDefault()?.PaymentDate,
                    LastPaymentAmount = paymentList.OrderByDescending(p => p.PaymentDate).FirstOrDefault()?.AmountPaid
                };

                var paginatedPayments = paymentDtos
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                var response = new PaymentHistoryResponseDto
                {
                    Payments = new PaginatedResponse<PaymentResponseDto>
                    {
                        Data = paginatedPayments,
                        TotalCount = paymentDtos.Count,
                        PageNumber = query.PageNumber,
                        PageSize = query.PageSize,
                        TotalPages = (int)Math.Ceiling(paymentDtos.Count / (double)query.PageSize),
                        HasPrevious = query.PageNumber > 1,
                        HasNext = query.PageNumber < (int)Math.Ceiling(paymentDtos.Count / (double)query.PageSize)
                    },
                    Summary = summary
                };

                return ApiResponse<PaymentHistoryResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment history for account {AccountId}", accountId);
                return ApiResponse<PaymentHistoryResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PaymentResponseDto>> ReversePaymentAsync(string accountId, string paymentId, ReversePaymentDto dto)
        {
            try
            {
                var account = await _repository.GetAccountWithPaymentsAsync(accountId);
                if (account == null)
                    return ApiResponse<PaymentResponseDto>.Fail($"Account with ID {accountId} not found");

                var payment = account.PaymentHistory.FirstOrDefault(p => p.PaymentId == paymentId);
                if (payment == null)
                    return ApiResponse<PaymentResponseDto>.Fail($"Payment {paymentId} not found");

                // Reverse the payment
                await _repository.UpdateOutstandingBalanceAsync(accountId, account.OutstandingBalance + payment.PrincipalPaid + payment.FeesPaid);

                payment.Status = PaymentStatus.Reversed;
                payment.Notes = $"{payment.Notes}\nReversed: {dto.ReversalReason} by {dto.ReversedBy} on {dto.ReversalDate}";

                await _repository.UpdateAsync(account);

                var response = _mapper.Map<PaymentResponseDto>(payment);
                response.RemainingBalance = account.OutstandingBalance + payment.PrincipalPaid + payment.FeesPaid;

                return ApiResponse<PaymentResponseDto>.Ok(response, "Payment reversed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reversing payment {PaymentId} for account {AccountId}", paymentId, accountId);
                return ApiResponse<PaymentResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Financial Operations

        public async Task<ApiResponse<AccountResponseDto>> UpdateOutstandingBalanceAsync(string id, UpdateBalanceDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var success = await _repository.UpdateOutstandingBalanceAsync(id, dto.NewOutstandingBalance);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to update balance");

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Balance updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating balance for account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> AdjustAvailableCreditAsync(string id, AdjustCreditDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var success = await _repository.UpdateAvailableCreditAsync(id, dto.NewAvailableCredit);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to adjust credit");

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Credit limit adjusted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjusting credit for account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> ApplyLateFeeAsync(string id, ApplyLateFeeDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                var success = await _repository.RecordLatePaymentAsync(id, dto.DaysOverdue, dto.LateFeeAmount);

                if (!success)
                    return ApiResponse<AccountResponseDto>.Fail("Failed to apply late fee");

                if (dto.AddToBalance)
                    await _repository.UpdateOutstandingBalanceAsync(id, account.OutstandingBalance + dto.LateFeeAmount);

                account = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, $"Late fee of {dto.LateFeeAmount:C} applied successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying late fee for account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> RestructurePaymentAsync(string id, RestructurePaymentDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                switch (dto.RestructuringType.ToLower())
                {
                    case "deferment":
                        if (dto.DeferralMonths.HasValue && account.NextPaymentDueDate.HasValue)
                        {
                            account.NextPaymentDueDate = account.NextPaymentDueDate.Value.AddMonths(dto.DeferralMonths.Value);
                            await _repository.UpdateAsync(account);
                        }
                        break;
                    case "reducedpayment":
                        if (dto.ReducedPaymentAmount.HasValue)
                        {
                            account.EmiAmount = dto.ReducedPaymentAmount.Value;
                            await _repository.UpdateAsync(account);
                        }
                        break;
                    case "skippayment":
                        if (dto.NewPaymentDate.HasValue)
                        {
                            account.NextPaymentDueDate = dto.NewPaymentDate.Value;
                            await _repository.UpdateAsync(account);
                        }
                        break;
                }

                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Payment restructured successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restructuring payment for account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Disbursement Operations

        public async Task<ApiResponse<AccountResponseDto>> DisburseAmountAsync(string id, DisburseAmountDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                if (account.Status != AccountStatus.Approved && account.Status != AccountStatus.PendingApproval)
                    return ApiResponse<AccountResponseDto>.Fail($"Account cannot be disbursed from status {account.Status}");

                account.DisbursedAmount = dto.Amount;
                account.Status = AccountStatus.Disbursed;
                account.NextPaymentDueDate = dto.DisbursementDate.AddMonths(1);
                account.UpdatedAt = DateTime.UtcNow;
                account.UpdatedBy = dto.DisbursedBy;

                if (account.Metadata == null)
                    account.Metadata = new AccountMetadata();

                account.Metadata.DisbursementReference = dto.TransactionReference;
                account.Metadata.ApprovalReference = dto.ApprovalReference;

                await _repository.UpdateAsync(account);

                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, "Amount disbursed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disbursing amount for account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountResponseDto>> ConfirmDisbursementAsync(string id, ConfirmDisbursementDto dto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                    return ApiResponse<AccountResponseDto>.Fail($"Account with ID {id} not found");

                account.UpdatedAt = DateTime.UtcNow;
                account.UpdatedBy = dto.ConfirmedBy;

                if (account.Metadata == null)
                    account.Metadata = new AccountMetadata();

                account.Metadata.DisbursementReference = dto.TransactionReference;

                if (!dto.IsSuccessful)
                {
                    account.Status = AccountStatus.PendingApproval;
                    account.Notes = dto.FailureReason;
                }

                await _repository.UpdateAsync(account);

                var response = _mapper.Map<AccountResponseDto>(account);
                return ApiResponse<AccountResponseDto>.Ok(response, dto.IsSuccessful ? "Disbursement confirmed" : "Disbursement failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming disbursement for account {Id}", id);
                return ApiResponse<AccountResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Statistics and Reports

        public async Task<ApiResponse<AccountStatisticsDto>> GetStatisticsAsync(AccountStatisticsQueryDto query)
        {
            try
            {
                var statisticsByStatus = await _repository.GetAccountStatisticsByStatusAsync();
                var statisticsByType = await _repository.GetAccountStatisticsByTypeAsync();
                var balanceByStatus = await _repository.GetBalanceSummaryByStatusAsync();
                var balanceByType = await _repository.GetBalanceSummaryByTypeAsync();

                var totalAccounts = statisticsByStatus.Values.Sum();
                var activeAccounts = statisticsByStatus
                    .Where(s => s.Key == AccountStatus.Active || s.Key == AccountStatus.Disbursed)
                    .Sum(s => s.Value);
                var delinquentAccounts = statisticsByStatus.GetValueOrDefault(AccountStatus.Delinquent, 0);
                var closedAccounts = statisticsByStatus.GetValueOrDefault(AccountStatus.Closed, 0);
                var writtenOffAccounts = statisticsByStatus.GetValueOrDefault(AccountStatus.WrittenOff, 0);

                var response = new AccountStatisticsDto
                {
                    TotalAccounts = totalAccounts,
                    ActiveAccounts = activeAccounts,
                    DelinquentAccounts = delinquentAccounts,
                    ClosedAccounts = closedAccounts,
                    WrittenOffAccounts = writtenOffAccounts,
                    DefaultedAccounts = statisticsByStatus.GetValueOrDefault(AccountStatus.Defaulted, 0),
                    TotalOutstandingBalance = balanceByStatus.Values.Sum(),
                    TotalDisbursedAmount = 0, // Would need separate calculation
                    TotalPrincipalPaid = 0, // Would need separate calculation
                    TotalInterestPaid = 0, // Would need separate calculation
                    TotalInterestEarned = 0, // Would need separate calculation
                    TotalLateFeesCollected = 0, // Would need separate calculation
                    AverageOutstandingBalance = totalAccounts > 0 ? balanceByStatus.Values.Sum() / totalAccounts : 0,
                    AverageDisbursedAmount = 0,
                    AverageInterestRate = 0,
                    AverageTermMonths = 0,
                    AverageEMI = 0,
                    AccountsByStatus = statisticsByStatus.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
                    AccountsByType = statisticsByType.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
                    BalanceByProductType = new Dictionary<string, decimal>(),
                    AsOfDate = DateTime.UtcNow,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate
                };

                return ApiResponse<AccountStatisticsDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting account statistics");
                return ApiResponse<AccountStatisticsDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AccountSummaryDto>> GetAccountSummaryAsync(string? customerId = null)
        {
            try
            {
                IEnumerable<Account> accounts;

                if (!string.IsNullOrEmpty(customerId))
                    accounts = await _repository.GetByCustomerIdAsync(customerId);
                else
                    accounts = await _repository.GetAllAsync();

                var accountList = accounts.ToList();
                var activeAccounts = accountList.Count(a => a.Status == AccountStatus.Active || a.Status == AccountStatus.Disbursed);
                var closedAccounts = accountList.Count(a => a.Status == AccountStatus.Closed);

                var response = new AccountSummaryDto
                {
                    CustomerId = customerId,
                    TotalAccounts = accountList.Count,
                    ActiveAccounts = activeAccounts,
                    ClosedAccounts = closedAccounts,
                    TotalOutstandingBalance = accountList.Sum(a => a.OutstandingBalance),
                    TotalAvailableCredit = accountList.Sum(a => a.AvailableCredit),
                    TotalDisbursedAmount = accountList.Sum(a => a.DisbursedAmount),
                    TotalMonthlyObligation = accountList.Sum(a => a.EmiAmount),
                    TotalPrincipalPaid = accountList.Sum(a => a.PrincipalAmount - a.OutstandingBalance),
                    TotalInterestPaid = accountList.Sum(a => a.PaymentHistory.Sum(p => p.InterestPaid)),
                    Accounts = _mapper.Map<List<AccountSummaryItemDto>>(accountList),
                    PaymentBehavior = new PaymentBehaviorDto
                    {
                        OnTimePayments = accountList.Sum(a => a.PaymentHistory.Count(p => p.LateDays == 0)),
                        LatePayments = accountList.Sum(a => a.PaymentHistory.Count(p => p.LateDays > 0)),
                        MissedPayments = accountList.Sum(a => a.PaymentHistory.Count(p => p.Status == PaymentStatus.Missed)),
                        IsHighRisk = accountList.Count(a => a.IsDelinquent) > accountList.Count * 0.3m
                    }
                };

                return ApiResponse<AccountSummaryDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting account summary for customer {CustomerId}", customerId ?? "All");
                return ApiResponse<AccountSummaryDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PortfolioPerformanceDto>> GetPortfolioPerformanceAsync(PerformanceQueryDto query)
        {
            try
            {
                var allAccounts = await _repository.GetAllAsync();
                var accounts = allAccounts.Where(a => a.CreatedAt >= query.FromDate && a.CreatedAt <= query.ToDate).ToList();

                var activeAccounts = accounts.Where(a => a.Status == AccountStatus.Active || a.Status == AccountStatus.Disbursed).ToList();
                var delinquentAccounts = accounts.Where(a => a.IsDelinquent).ToList();

                var totalDisbursements = accounts.Sum(a => a.DisbursedAmount);
                var totalCollections = accounts.Sum(a => a.PaymentHistory.Sum(p => p.AmountPaid));
                var totalInterestEarned = accounts.Sum(a => a.PaymentHistory.Sum(p => p.InterestPaid));
                var totalLateFees = accounts.Sum(a => a.PaymentHistory.Sum(p => p.FeesPaid));

                var portfolioAtRisk30Days = activeAccounts.Where(a => a.DaysOverdue >= 30).Sum(a => a.OutstandingBalance);
                var portfolioAtRisk60Days = activeAccounts.Where(a => a.DaysOverdue >= 60).Sum(a => a.OutstandingBalance);
                var portfolioAtRisk90Days = activeAccounts.Where(a => a.DaysOverdue >= 90).Sum(a => a.OutstandingBalance);

                var response = new PortfolioPerformanceDto
                {
                    TotalPortfolioValue = totalDisbursements - totalCollections,
                    TotalDisbursements = totalDisbursements,
                    TotalCollections = totalCollections,
                    TotalInterestEarned = totalInterestEarned,
                    TotalFeesCollected = totalLateFees,
                    PortfolioAtRisk30Days = portfolioAtRisk30Days,
                    PortfolioAtRisk60Days = portfolioAtRisk60Days,
                    PortfolioAtRisk90Days = portfolioAtRisk90Days,
                    PortfolioAtRiskPercentage = totalDisbursements > 0
                        ? (portfolioAtRisk30Days / totalDisbursements) * 100
                        : 0,
                    DelinquencyRate = activeAccounts.Any()
                        ? (decimal)delinquentAccounts.Count / activeAccounts.Count * 100
                        : 0,
                    DefaultRate = accounts.Any()
                        ? (decimal)accounts.Count(a => a.Status == AccountStatus.Defaulted) / accounts.Count * 100
                        : 0,
                    PrepaymentRate = 0,
                    CollectionEfficiency = totalDisbursements > 0
                        ? (totalCollections / totalDisbursements) * 100
                        : 0,
                    AsOfDate = query.ToDate,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate
                };

                return ApiResponse<PortfolioPerformanceDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio performance");
                return ApiResponse<PortfolioPerformanceDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Bulk Operations

        public async Task<ApiResponse<BulkOperationResponseDto>> BulkUpdateStatusAsync(BulkStatusUpdateDto dto)
        {
            try
            {
                var success = await _repository.BulkUpdateStatusAsync(
                    dto.AccountIds,
                    Enum.Parse<AccountStatus>(dto.NewStatus),
                    dto.UpdateReason,
                    dto.UpdatedBy);

                var result = new BulkOperationResponseDto
                {
                    TotalProcessed = dto.AccountIds.Count,
                    Successful = success ? dto.AccountIds.Count : 0,
                    Failed = success ? 0 : dto.AccountIds.Count,
                    ProcessedAt = DateTime.UtcNow,
                    ProcessedBy = dto.UpdatedBy
                };

                if (success)
                    result.SuccessfulIds.AddRange(dto.AccountIds);
                else
                    result.FailedIds.AddRange(dto.AccountIds);

                return ApiResponse<BulkOperationResponseDto>.Ok(result, $"Processed {result.Successful} of {result.TotalProcessed} accounts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk status update");
                return ApiResponse<BulkOperationResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<BulkOperationResponseDto>> BulkMarkOverdueAccountsAsync(BulkOverdueDto dto)
        {
            try
            {
                var count = await _repository.BulkMarkOverdueAccountsAsync(dto.AsOfDate);

                var result = new BulkOperationResponseDto
                {
                    TotalProcessed = count,
                    Successful = count,
                    ProcessedAt = DateTime.UtcNow,
                    ProcessedBy = dto.MarkedBy
                };

                return ApiResponse<BulkOperationResponseDto>.Ok(result, $"Marked {count} accounts as overdue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk marking overdue accounts");
                return ApiResponse<BulkOperationResponseDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Validation and Utilities

        public async Task<ApiResponse<bool>> ValidateAccountForDisbursementAsync(string accountId)
        {
            try
            {
                var isValid = await _repository.IsAccountActiveAsync(accountId);
                var account = await _repository.GetByIdAsync(accountId);

                if (account == null)
                    return ApiResponse<bool>.Fail($"Account with ID {accountId} not found");

                isValid = isValid && (account.Status == AccountStatus.Approved || account.Status == AccountStatus.PendingApproval);

                return ApiResponse<bool>.Ok(isValid, isValid ? "Account is ready for disbursement" : "Account is not ready for disbursement");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating account for disbursement {AccountId}", accountId);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<decimal>> CalculateEMIAsync(CalculateEMIDto dto)
        {
            try
            {
                decimal emi = 0;

                if (dto.InterestType.ToLower() == "reducing")
                {
                    var monthlyRate = dto.InterestRate / 1200;
                    var power = (decimal)Math.Pow(1 + (double)monthlyRate, dto.TermMonths);
                    emi = dto.PrincipalAmount * monthlyRate * power / (power - 1);
                }
                else if (dto.InterestType.ToLower() == "flat")
                {
                    var totalInterest = dto.PrincipalAmount * (dto.InterestRate / 100) * (dto.TermMonths / 12m);
                    emi = (dto.PrincipalAmount + totalInterest) / dto.TermMonths;
                }

                return ApiResponse<decimal>.Ok(Math.Round(emi, 2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating EMI");
                return ApiResponse<decimal>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<AmortizationScheduleDto>> CalculateAmortizationScheduleAsync(CalculateAmortizationDto dto)
        {
            try
            {
                var schedule = new AmortizationScheduleDto
                {
                    PrincipalAmount = dto.PrincipalAmount,
                    InterestRate = dto.InterestRate,
                    TermMonths = dto.TermMonths
                };

                var monthlyRate = dto.InterestRate / 1200;
                var power = (decimal)Math.Pow(1 + (double)monthlyRate, dto.TermMonths);
                var emi = dto.PrincipalAmount * monthlyRate * power / (power - 1);

                schedule.EmiAmount = Math.Round(emi, 2);

                var balance = dto.PrincipalAmount;
                var totalInterest = 0m;

                for (int i = 1; i <= dto.TermMonths; i++)
                {
                    var interest = balance * monthlyRate;
                    var principal = emi - interest;

                    if (principal > balance)
                        principal = balance;

                    balance -= principal;
                    totalInterest += interest;

                    schedule.Entries.Add(new AmortizationEntryDto
                    {
                        Period = i,
                        PaymentDate = dto.StartDate.AddMonths(i),
                        BeginningBalance = Math.Round(balance + principal, 2),
                        PaymentAmount = Math.Round(emi, 2),
                        PrincipalPaid = Math.Round(principal, 2),
                        InterestPaid = Math.Round(interest, 2),
                        EndingBalance = Math.Round(balance, 2),
                        IsCompleted = balance <= 0
                    });

                    if (balance <= 0)
                        break;
                }

                schedule.TotalInterest = Math.Round(totalInterest, 2);
                schedule.TotalPayment = Math.Round(dto.PrincipalAmount + totalInterest, 2);

                return ApiResponse<AmortizationScheduleDto>.Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating amortization schedule");
                return ApiResponse<AmortizationScheduleDto>.Fail(ex.Message);
            }
        }

        #endregion

        #region Helper Methods

        private Expression<Func<Account, bool>> BuildFilter(AccountQueryDto query)
        {
            return x => (string.IsNullOrEmpty(query.CustomerId) || x.CustomerId == query.CustomerId) &&
                        (string.IsNullOrEmpty(query.AccountStatus) || x.Status.ToString() == query.AccountStatus) &&
                        (string.IsNullOrEmpty(query.AccountType) || x.AccountType.ToString() == query.AccountType) &&
                        (string.IsNullOrEmpty(query.ProductType) || x.ProductType == query.ProductType) &&
                        (!query.FromDate.HasValue || x.CreatedAt >= query.FromDate) &&
                        (!query.ToDate.HasValue || x.CreatedAt <= query.ToDate) &&
                        (!query.MinBalance.HasValue || x.OutstandingBalance >= query.MinBalance) &&
                        (!query.MaxBalance.HasValue || x.OutstandingBalance <= query.MaxBalance) &&
                        (string.IsNullOrEmpty(query.BranchCode) || x.BranchCode == query.BranchCode) &&
                        (string.IsNullOrEmpty(query.AssignedOfficer) || x.AssignedOfficer == query.AssignedOfficer) &&
                        (!query.IsDelinquent.HasValue || x.IsDelinquent == query.IsDelinquent) &&
                        (!query.MinOverdueDays.HasValue || x.DaysOverdue >= query.MinOverdueDays);
        }

        private (decimal principal, decimal interest, decimal fees) AllocatePayment(Account account, decimal paymentAmount)
        {
            var feesPaid = Math.Min(
                account.PaymentHistory.Where(p => p.Status == PaymentStatus.Overdue).Sum(p => p.LateFeeCharged),
                paymentAmount);
            var remainingAfterFees = paymentAmount - feesPaid;

            var interestPaid = Math.Min(
                account.OutstandingBalance * (account.InterestRate / 1200),
                remainingAfterFees);
            var remainingAfterInterest = remainingAfterFees - interestPaid;

            var principalPaid = Math.Min(account.OutstandingBalance, remainingAfterInterest);

            return (principalPaid, interestPaid, feesPaid);
        }

        private List<PaymentInstallmentDto> GeneratePaymentSchedule(Account account)
        {
            var schedule = new List<PaymentInstallmentDto>();
            var balance = account.PrincipalAmount;
            var monthlyRate = account.InterestRate / 1200;
            var startDate = account.OpeningDate;

            for (int i = 1; i <= account.TermMonths; i++)
            {
                var interest = balance * monthlyRate;
                var principal = account.EmiAmount - interest;

                if (principal > balance)
                    principal = balance;

                balance -= principal;

                var dueDate = startDate.AddMonths(i);
                var existingPayment = account.PaymentHistory.FirstOrDefault(p => p.DueDate.Date == dueDate.Date);

                schedule.Add(new PaymentInstallmentDto
                {
                    InstallmentNumber = i,
                    DueDate = dueDate,
                    AmountDue = account.EmiAmount,
                    PrincipalAmount = Math.Round(principal, 2),
                    InterestAmount = Math.Round(interest, 2),
                    BalanceAfterPayment = Math.Round(balance, 2),
                    Status = existingPayment?.Status.ToString() ?? (dueDate < DateTime.UtcNow ? "Overdue" : "Upcoming"),
                    AmountPaid = existingPayment?.AmountPaid,
                    PaidDate = existingPayment?.PaymentDate,
                    LateDays = existingPayment?.LateDays,
                    LateFeeCharged = existingPayment?.LateFeeCharged
                });

                if (balance <= 0)
                    break;
            }

            return schedule;
        }

        private AgingBucketSummaryDto GetBucketSummary(List<AccountAgingReportDto> accounts, Func<AccountAgingReportDto, bool> predicate)
        {
            var filtered = accounts.Where(predicate).ToList();

            return new AgingBucketSummaryDto
            {
                AccountCount = filtered.Count,
                TotalOutstandingBalance = filtered.Sum(a => a.OutstandingBalance),
                TotalOverdueAmount = filtered.Sum(a => a.OverdueAmount),
                PercentageOfPortfolio = accounts.Count > 0
                    ? (decimal)filtered.Sum(a => a.OutstandingBalance) / accounts.Sum(a => a.OutstandingBalance) * 100
                    : 0
            };
        }

        private decimal CalculateOverdueAmount(Account account)
        {
            if (!account.NextPaymentDueDate.HasValue || account.NextPaymentDueDate.Value > DateTime.UtcNow)
                return 0;

            var overduePayments = account.PaymentHistory
                .Where(p => p.DueDate < DateTime.UtcNow && p.Status != PaymentStatus.Paid)
                .Sum(p => p.AmountDue - p.AmountPaid);

            return overduePayments;
        }

        #endregion
    }
}