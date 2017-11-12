using System.ComponentModel.DataAnnotations;

namespace Restaurant.Entities
{
    public class BasicEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string NameAr { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}