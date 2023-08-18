namespace DAM.Domain.Configurations
{
    public class RedisCacheConfiguration
    {
        public string? RedisCacheServer { get; set; }
        public int PortNumber { get; set; }
        public int AbsoluteExpirationInHours { get; set; }
        public int SlidingExpirationInHours { get; set; }
    }
}
