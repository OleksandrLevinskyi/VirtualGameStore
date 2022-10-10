namespace VirtualGameStore.Models
{
    public class GameCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public IEnumerable<User> Users { get; set; }
    }
}
