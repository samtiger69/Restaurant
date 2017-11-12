using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// meal attribute create model
    /// </summary>
    public class MealAttributeCreate
    {
        /// <summary>
        /// meal id
        /// </summary>
        [Required]
        public int MealId { get; set; }

        /// <summary>
        /// attribute id
        /// </summary>
        [Required]
        public int AttributeId { get; set; }
    }
}