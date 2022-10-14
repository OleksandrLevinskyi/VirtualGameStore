using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using VirtualGameStore.Models.ValidationAttributes;

namespace VirtualGameStore.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public string? CreatorId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [ValidDateTime]
        public DateTime DateTime { get; set; }

        public User? Creator { get; set; }

        public List<Registration>? Registrations { get; set; }
    }
}
