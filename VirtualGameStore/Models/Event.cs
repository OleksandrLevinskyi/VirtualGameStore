using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VirtualGameStore.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public User? Creator { get; set; }

        public List<Registration>? Registrations { get; set; }
    }
}
