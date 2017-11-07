using Restaurant.Entities;

namespace Restaurant.Models
{
    public class MealTypeUpdate : BasicEntity
    {
        public string Description { get; set; }
        public int? BranchId { get; set; }
    }
}