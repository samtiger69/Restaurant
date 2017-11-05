using System.Collections.Generic;

namespace Restaurant.Entities
{
    public class AttributeGroup : BasicEntity
    {
        public bool IsDeleted { get; set; }
        public virtual List<Attribute> Attributes { get; set; }
    }
}