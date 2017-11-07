using static Restaurant.Models.Enums;

namespace Restaurant.Models
{
    public class OrderUpdate
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}