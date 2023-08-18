using DAM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAM.Domain.Entities
{
    public class Asset : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }
        [Required]
        public bool IsPrivate { get; set; } = false;
        public AccessType Type { get; set; }
        [Required]
        public User Owner { get; set; }
    }
}
