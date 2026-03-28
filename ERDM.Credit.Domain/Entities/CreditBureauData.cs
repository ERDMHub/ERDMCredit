namespace ERDM.Credit.Domain.Entities
{
    public class CreditBureauData
    {
        public string Bureau { get; set; }
        public int Score { get; set; }
        public DateTime ScoreDate { get; set; }
        public string InquiryId { get; set; }
    }
}
