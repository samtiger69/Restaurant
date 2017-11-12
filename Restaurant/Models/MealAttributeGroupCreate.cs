using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// meal attributeGroup create model
    /// </summary>
    public class MealAttributeGroupCreate
    {
        /// <summary>
        /// meal id
        /// </summary>
        [Required]
        public int MealId { get; set; }

        /// <summary>
        /// Attribute group id
        /// </summary>
        [Required]
        public int AttributeGroupId { get; set; }
    }
}