using System.ComponentModel.DataAnnotations;
using static Restaurant.Models.Enums;

namespace Restaurant.Models
{
    /// <summary>
    /// image create model
    /// </summary>
    public class ImageCreate
    {
        /// <summary>
        /// meal or mealtype id
        /// </summary>
        [Required]
        public int SourceId { get; set; }

        /// <summary>
        /// int represting sourceType
        /// </summary>
        [Required]
        public SourceType SourceType { get; set; }

        /// <summary>
        /// base64 string
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// default image
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }
    }
}