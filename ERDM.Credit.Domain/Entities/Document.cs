namespace ERDM.Credit.Domain.Entities
{
    public class Document
    {
        public string DocumentId { get; set; }
        public string Type { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public bool Verified { get; set; }
    }
}
