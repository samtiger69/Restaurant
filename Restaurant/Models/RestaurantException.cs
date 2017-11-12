using System;

namespace Restaurant.Models
{
    /// <summary>
    /// custom exception
    /// </summary>
    public class RestaurantException : Exception
    {
        /// <summary>
        /// error code
        /// </summary>
        public ErrorCode ErrorCode { get; set; }
    }
}