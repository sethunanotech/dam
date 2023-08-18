namespace DAM.Domain.Configurations
{
    public class SqlServerCacheConfiguration
    {
        public int AbsoluteExpirationInHours { get; set; }
        public int SlidingExpirationInHours { get; set; }
    }
}
