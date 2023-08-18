using System.ComponentModel.DataAnnotations;

namespace DAM.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        
    }
}
