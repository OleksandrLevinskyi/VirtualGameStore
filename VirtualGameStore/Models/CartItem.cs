namespace VirtualGameStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GameId { get; set; }
        public int Quantity { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }
    }
}
