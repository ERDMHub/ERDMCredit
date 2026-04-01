namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class BulkOperationErrorDto
    {
        public string? Id { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
