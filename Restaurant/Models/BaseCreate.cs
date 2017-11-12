using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// base create model
    /// </summary>
    public class BaseCreate
    {
        /// <summary>
        /// English name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Arabic Name
        /// </summary>
        [Required]
        public string NameAr { get; set; }
    }
}