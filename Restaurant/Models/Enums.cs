namespace Restaurant.Models
{
    public class Enums
    {
        public enum ImageType
        {
            Unspecified = 0,
            MealType = 1,
            Meal = 2
        }

        public enum OrderStatus
        {
            Draft = 0,
            New = 1,
            Seen = 2,
            Confirmed = 3,
            Rejected = 4,
            OnDelivery = 5,
            Delivered = 6,
            Blocked = 8
        }

        public enum ActionLog
        {
            CreateOrder = 1,
            UpdateOrder = 2,

            CraeteMeal = 3,
            UpdateMeal = 4,

            CreateMealType = 5,
            UpdateMealType = 6,

            CreateAttributeGroup = 7,
            UpdateAttributeGroup = 8,

            CreateAttribute = 9,
            UpdateAttribute = 10,

            CreateBranch = 11,
            UpdateBranch = 12
        }

        public enum SourceType
        {
            Branch = 1,
            MealType = 2,
            Meal = 3,
            AttributeGroup = 4,
            Attribute = 5,
            Order = 6
        }

    }
}