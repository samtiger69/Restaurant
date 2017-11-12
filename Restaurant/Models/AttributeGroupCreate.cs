using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// attribute group create model
    /// </summary>
    public class AttributeGroupCreate : BaseCreate
    {
        /// <summary>
        /// array of attribute ids
        /// </summary>
        [Required]
        public virtual List<int> Attributes { get; set; }
    }
}