using DAM.Domain.Enums;

namespace DAM.Application.DTO.Requests
{
    public record OrganizationRequest
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        public AccessType AccessType { get; set; }
    }
}
