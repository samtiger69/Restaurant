using System.Collections.Generic;

namespace Restaurant.Entities
{
    public class Meal : BasicEntity
    {
        public int MealTypeId { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public virtual List<Attribute> Attributes { get; set; }
        public virtual List<AttributeGroup> AttributeGroups { get; set; }
    }
}