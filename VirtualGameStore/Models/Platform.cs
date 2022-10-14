using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Game> Games { get; set; }
    }
}
