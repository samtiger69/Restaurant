using Restaurant.Entities;

namespace Restaurant.Models
{
    public class AttributeUpdate : BasicEntity
    {
        public int? GroupId { get; set; }
        public decimal? Price { get; set; }
    }
}