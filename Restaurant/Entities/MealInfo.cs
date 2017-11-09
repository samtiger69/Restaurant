using System.Collections.Generic;

namespace Restaurant.Entities
{
    public class MealInfo
    {
        public MealInfo()
        {
            Attributes = new List<Attribute>();
            AttributeGroups = new List<AttributeGroup>();
        }
        public virtual List<AttributeGroup> AttributeGroups { get; set; }
        public virtual List<Attribute> Attributes { get; set; }
    }
}