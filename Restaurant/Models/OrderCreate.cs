using System.Collections.Generic;

namespace Restaurant.Models
{
    public class OrderCreate
    {
        public string UserId { get; set; }
        public int BranchId { get; set; }
        public virtual List<OrderMeal> OrderMeals { get; set; }
        public string Notes { get; set; }
    }
    public class OrderMeal
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int Quantity { get; set; }
        public virtual List<int> AttributeId { get; set; }
    }
}