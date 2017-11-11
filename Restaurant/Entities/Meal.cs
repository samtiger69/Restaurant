namespace Restaurant.Entities
{
    public class Meal : BasicEntity
    {
        public int MealTypeId { get; set; }
        public decimal Price { get; set; }
        public int DefaultImageId { get; set; }
    }
}