using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class ReviewsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetOverallRating_ThereAreAnyApprovedReviews_ReturnsTheirOverallRating()
        {
            Game game = new Game()
            {
                Reviews = new List<Review>()
                {
                    new Review(){IsApproved=true,Rating=5},
                    new Review(){IsApproved=true,Rating=2},
                    new Review(){IsApproved=null,Rating=3.5f},
                    new Review(){IsApproved=false,Rating=1},
                    new Review(){IsApproved=true,Rating=0.5f},
                }
            };

            float overallRating = game.GetOverallRating();

            Assert.That(overallRating, Is.EqualTo(2.5f));
        }

        [Test]
        public void GetOverallRating_ThereAreNoApprovedReviews_ReturnsZeroAsOverallRating()
        {
            Game game = new Game()
            {
                Reviews = new List<Review>()
                {
                    new Review(){IsApproved=false,Rating=5},
                    new Review(){IsApproved=null,Rating=2},
                    new Review(){IsApproved=null,Rating=3.5f},
                    new Review(){IsApproved=false,Rating=1},
                    new Review(){IsApproved=null,Rating=0.5f},
                }
            };

            float overallRating = game.GetOverallRating();

            Assert.That(overallRating, Is.EqualTo(0));
        }

        [Test]
        public void GetOverallRating_ThereAreNoGameReviews_ReturnsZeroAsOverallRating()
        {
            Game game = new Game();

            float overallRating = game.GetOverallRating();

            Assert.That(overallRating, Is.EqualTo(0));
        }
    }
}