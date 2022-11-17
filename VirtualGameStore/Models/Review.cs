using System.ComponentModel.DataAnnotations;
using VirtualGameStore.Models.ValidationAttributes;

namespace VirtualGameStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public string AuthorId { get; set; }
        public float Rating { get; set; }
        public string? Comment { get; set; }
        public bool? IsApproved { get; set; } = null;

        [Display(Name = "Created On/At")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime DateTime { get; set; }
        public Game? Game { get; set; }
        public User? Author { get; set; }

        public bool IsCommentProvided()
        {
            return Comment != null;
        }
    }
}
