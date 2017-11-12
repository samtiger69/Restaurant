using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// order create model
    /// </summary>
    public class OrderCreate
    {
        /// <summary>
        /// order user id
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// branch id
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// array of meals
        /// </summary>
        [Required]
        public virtual List<OrderMeal> OrderMeals { get; set; }

        /// <summary>
        /// optional notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// is pick up
        /// </summary>
        [Required]
        public bool IsPickUp { get; set; }

        /// <summary>
        /// required in case of not pickUp
        /// </summary>
        public OrderDeliveryAddress Address { get; set; }
    }
    public class OrderMeal
    {
        /// <summary>
        /// do not send
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// meal id
        /// </summary>
        [Required]
        public int MealId { get; set; }

        /// <summary>
        /// meal quantity
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// array of attribute ids
        /// </summary>
        public virtual List<int> AttributeId { get; set; }
    }

    public class OrderDeliveryAddress
    {
        /// <summary>
        /// area name
        /// </summary>
        [Required]
        public string Area { get; set; }

        /// <summary>
        /// street name
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        /// building number/name
        /// </summary>
        [Required]
        public string Building { get; set; }

        /// <summary>
        /// floor number
        /// </summary>
        [Required]
        public string Floor { get; set; }

        /// <summary>
        /// office number
        /// </summary>
        [Required]
        public string OfficeNumber { get; set; }
    }
}