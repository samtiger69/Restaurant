using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// base update model
    /// </summary>
    public class BaseUpdate
    {
        /// <summary>
        /// object id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// English name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Arabic name
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// Enable/Disable
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// delete object
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}