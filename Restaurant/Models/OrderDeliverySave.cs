using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// order delivery object
    /// </summary>
    public class OrderDeliverySave
    {
        /// <summary>
        /// order id
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// delivery user id
        /// </summary>
        [Required]
        public int DeliveryUserId { get; set; }
    }
}