namespace ERDM.Credit.Contracts.DTOs
{
    public class ApplicationMetadataDto
    {
        public ApplicationMetadataDto()
        {
            
        }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string DeviceId { get; set; }
        public string Source { get; set; }
    }
}
