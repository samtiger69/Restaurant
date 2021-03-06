﻿using static Restaurant.Models.Enums;

namespace Restaurant.Entities
{
    public class Order : BaseEntity
    {
        public int BranchId { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public string Notes { get; set; }
    }
}