using Restaurant.Entities;

namespace Restaurant.Models
{
    public class MealUpdate : BasicEntity
    {
        public int? MealTypeId { get; set; }
        public decimal? Price { get; set; }
        public int? DefaultImageId { get; set; }
    }
}