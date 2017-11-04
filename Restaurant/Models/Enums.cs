using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class Enums
    {
        public enum ImageType
        {
            Unspecified = 0,
            MealType = 1,
            Meal = 2
        }

        public enum OrderStatus
        {
            Draft = 0,
            New = 1,
            Seen = 2,
            Confirmed = 3,
            Rejected = 4,
            OnDelivery = 5,
            Delivered = 6,
            Blocked = 8
        }
    }
}