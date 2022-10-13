namespace VirtualGameStore.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public IEnumerable<User> Users { get; set; }
    }
}
