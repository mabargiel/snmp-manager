namespace SNMPManager.Models
{
    public class SonarDto
    {
        public SonarDto(string id, string title, string ipAddress, string mib, bool isActive)
        {
            Id = id;
            Title = title;
            IpAddress = ipAddress;
            Mib = mib;
            IsActive = isActive;
        }

        public string Id { get; }
        public string Title { get; }
        public string IpAddress { get; }
        public string Mib { get; }
        public bool IsActive { get; }
    }
}