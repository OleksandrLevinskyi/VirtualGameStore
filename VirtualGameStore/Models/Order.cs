namespace VirtualGameStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BillingAddressId { get; set; }
        public int ShippingAddressId { get; set; }
        public int StatusId { get; set; }

        public bool IsProcessed { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public User User { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }

        public double Total { get => Items.Aggregate(0.0, (total, orderItem) => total + orderItem.UnitPrice * orderItem.Quantity); }
    }
}
