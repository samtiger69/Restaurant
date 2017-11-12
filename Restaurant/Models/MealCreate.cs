using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// meal create model
    /// </summary>
    public class MealCreate : BaseCreate
    {
        /// <summary>
        /// Meal type id
        /// </summary>
        [Required]
        public int MealTypeId { get; set; }

        /// <summary>
        /// mea price 2.34
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// array of attribute ids
        /// </summary>
        public virtual List<int> Attributes { get; set; }

        /// <summary>
        /// array of attributeGroup ids
        /// </summary>
        public virtual List<int> AttributeGroups { get; set; }

        /// <summary>
        /// array of images
        /// </summary>
        public virtual List<MealImageCreate> MealImages { get; set; }
    }
}