using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public float Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; }
        public int GameId { get;set; }
        public int UserId { get; set; }
    }
}
