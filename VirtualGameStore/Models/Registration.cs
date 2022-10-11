using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public DateTime DateTimeRegistered { get; set; } = DateTime.Now;

        public User? User { get; set; }

        public Event? Event { get; set; }
    }
}
