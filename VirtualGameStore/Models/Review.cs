using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public string AuthorId { get; set; }
        public float Rating { get; set; }
        public string Title { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; }
        public Game Game { get; set; }
        public User Author { get; set; }
    }
}
