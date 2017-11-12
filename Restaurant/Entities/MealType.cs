namespace Restaurant.Entities
{
    public class MealType : BasicEntity
    {
        public string Description { get; set; }
        public int BranchId { get; set; }
        public int ImageId { get; set; }
    }
}