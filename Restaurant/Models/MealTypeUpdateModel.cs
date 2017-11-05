using Restaurant.Entities;

namespace Restaurant.Models
{
    public class MealTypeUpdateModel : BasicEntity
    {
        public string Description { get; set; }
        public int? BranchId { get; set; }
    }
}