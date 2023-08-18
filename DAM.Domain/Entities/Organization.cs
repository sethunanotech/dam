using DAM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAM.Domain.Entities
{
    public class Organization : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public AccessType AccessType { get; set; }
    }
}
