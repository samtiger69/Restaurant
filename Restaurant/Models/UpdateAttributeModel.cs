using Restaurant.Entities;

namespace Restaurant.Models
{
    public class UpdateAttributeModel : BasicEntity
    {
        public int? GroupId { get; set; }
        public decimal? Price { get; set; }
    }
}