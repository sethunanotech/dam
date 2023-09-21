namespace DAM.Domain.MultiTenancy
{
    public class TenantMapping
    {
        public string? Default { get; set; }

        public Dictionary<string, string> Tenants { get; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    }
}
