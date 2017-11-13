using System;
using System.Collections.Generic;
using static Restaurant.Models.Enums;

namespace Restaurant.Models
{
    /// <summary>
    /// order list item model
    /// </summary>
    public class OrderListItem
    {
        /// <summary>
        /// order id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// order create date
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// order last update date
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// order total price
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// order current status
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// order created by
        /// </summary>
        public OrderUpdatedByInfo OrderedBy { get; set; }

        /// <summary>
        /// order last updated by
        /// </summary>
        public OrderUpdatedByInfo UpdatedBy { get; set; }

        /// <summary>
        /// array of order meals
        /// </summary>
        public virtual List<OrderListItemMeal> Meals { get; set; }
    }

    /// <summary>
    /// order list item meal
    /// </summary>
    public class OrderListItemMeal
    {
        /// <summary>
        /// meal id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// meal english name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// meal arabic name
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// meal price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// meal quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// list of order list item meal attributes
        /// </summary>
        public virtual List<OrderListItemMealAttribute> Attributes { get; set; }
    }

    /// <summary>
    /// order list item meal attribute
    /// </summary>
    public class OrderListItemMealAttribute
    {
        /// <summary>
        /// attribute id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// attribute english name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// attribute arabic name
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// attribute price
        /// </summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// order updated/created by info
    /// </summary>
    public class OrderUpdatedByInfo
    {
        /// <summary>
        /// user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// user english name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// user arabic name
        /// </summary>
        public string NameAr { get; set; }
    }

}