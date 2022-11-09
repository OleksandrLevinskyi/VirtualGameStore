namespace VirtualGameStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GameId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Game Game { get; set; }

        public double Total { get => UnitPrice * Quantity; }
    }
}
