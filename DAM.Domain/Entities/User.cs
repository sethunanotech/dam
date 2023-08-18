using DAM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAM.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [StringLength(150)]
        public string? EmailId { get; set; }
        public Organization? Organization { get; set; }
        public AccessPermissionLevel AccessPermissionLevel { get; set; }
    }
}
