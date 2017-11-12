using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// attribute create model
    /// </summary>
    public class AttributeCreate : BaseCreate
    {
        /// <summary>
        /// attribute group id
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// attribute price
        /// </summary>
        [Required]
        public decimal Price { get; set; }
    }
}