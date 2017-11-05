using System.Collections.Generic;

namespace Restaurant.Entities
{
    public class AttributeGroup : BasicEntity
    {
        public virtual List<Attribute> Attributes { get; set; }
    }
}