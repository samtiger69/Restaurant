﻿using System.ComponentModel.DataAnnotations;
using static Restaurant.Models.Enums;

namespace Restaurant.Models
{
    /// <summary>
    /// order update model
    /// </summary>
    public class OrderUpdate
    {
        /// <summary>
        /// order id
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// order status
        /// </summary>
        [Required]
        public OrderStatus Status { get; set; }
    }
}