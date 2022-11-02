using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Price { get; set; }

        [Display(Name = "Is Digital")]
        public bool IsDigital { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0.")]
        public int Stock { get; set; }

        public List<Review>? Reviews { get; set; }

        public List<Category>? Categories { get; set; }

        public List<Platform>? Platforms { get; set; }

        public bool IsAvailable()
        {
            return Stock > 0;
        }

        public float GetOverallRating()
        {
            if (Reviews == null)
            {
                return 0;
            }

            List<Review> approvedReviews = Reviews.Where(r => r.IsApproved == true).ToList();

            if (approvedReviews.Count <= 0)
            {
                return 0;
            }

            float totalScore = approvedReviews.Sum(r => r.Rating);
            int reviewCount = approvedReviews.Count;

            return (totalScore / reviewCount);
        }
    }
}
