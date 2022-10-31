namespace VirtualGameStore.Models
{
    public class OrderStatus
    {
        public static OrderStatus Processing
        {
            get => new() { Id = 1, Name = "Processing" };
        }

        public static OrderStatus Complete
        {
            get => new () { Id = 2, Name = "Complete" };
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
