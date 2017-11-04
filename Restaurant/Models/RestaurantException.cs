using System;

namespace Restaurant.Models
{
    public class RestaurantException : Exception
    {
        public ErrorCode ErrorCode { get; set; }
    }
}