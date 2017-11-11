using Restaurant.Entities;
using System.Collections.Generic;

namespace Restaurant.Models
{
    public class MealCreate : Meal
    {
        public virtual List<int> Attributes { get; set; }
        public virtual List<int> AttributeGroups { get; set; }
        public virtual List<MealImageCreate> MealImages { get; set; }
    }
}