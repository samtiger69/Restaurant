using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// meal type create model
    /// </summary>
    public class MealTypeCreate : BaseCreate
    {
        /// <summary>
        /// base64 image content
        /// </summary>
        [Required]
        public string ImageContent { get; set; }

        /// <summary>
        /// branch id
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// optional description
        /// </summary>
        public string Description { get; set; }
    }
}