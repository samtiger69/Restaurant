using Restaurant.Entities;

namespace Restaurant.Models
{
    public class UpdateMealModel : BasicEntity
    {
        public int? MealTypeId { get; set; }
        public decimal? Price { get; set; }
    }
}