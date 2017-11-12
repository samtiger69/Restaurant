using System.Collections.Generic;

namespace Restaurant.Models
{
    /// <summary>
    /// attribute group update model
    /// </summary>
    public class AttributeGroupUpdate : BaseUpdate
    {
        /// <summary>
        /// array of attribute ids to be added
        /// </summary>
        public virtual List<int> AttributesToAdd { get; set; }

        /// <summary>
        /// array of attribute ids to be removed
        /// </summary>
        public virtual List<int> AttributesToRemove { get; set; }
    }
}