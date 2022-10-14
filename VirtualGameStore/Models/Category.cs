using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Game> Games { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
