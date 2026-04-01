namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class ApplicationQueryDto
    {
        public ApplicationQueryDto()
        {
            
        }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Status { get; set; }
        public string CustomerId { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }
}
