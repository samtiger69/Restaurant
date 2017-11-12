using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// meal image create model
    /// </summary>
    public class MealImageCreate
    {
        /// <summary>
        /// base64 string
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// set default image
        /// </summary>
        [Required]
        public bool IsDefualt { get; set; }
    }
}