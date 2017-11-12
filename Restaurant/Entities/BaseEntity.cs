using System.ComponentModel.DataAnnotations;

namespace Restaurant.Entities
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }
    }
}