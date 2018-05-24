namespace SNMPManager.Models
{
    public class CreateSonarDto
    {
        public string Title { get; set; }
        public string IpAddress { get; set; }
        public string Mib { get; set; }
        public bool IsActive { get; set; }
    }
}