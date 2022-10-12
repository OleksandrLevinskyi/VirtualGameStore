using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public int? CategoryId { get; set; }
        
        public int? PlatformId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public double Price { get; set; }

        [Display(Name = "Is Digital")]
        public bool IsDigital { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0.")]
        public int Stock { get; set; }

        public Platform? Platform { get; set; }

        public Category? Category { get; set; }
    }
}
