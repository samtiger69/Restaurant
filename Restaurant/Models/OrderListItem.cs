using System;
using System.Collections.Generic;
using static Restaurant.Models.Enums;

namespace Restaurant.Models
{
    public class OrderListItem
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public OrderUpdatedByInfo OrderedBy { get; set; }
        public OrderUpdatedByInfo UpdatedBy { get; set; }
        public virtual List<OrderListItemMeal> Meals { get; set; }
    }

    public class OrderListItemMeal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual List<OrderListItemMealAttribute> Attributes { get; set; }
    }

    public class OrderListItemMealAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderUpdatedByInfo
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
    }

}