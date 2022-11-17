using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class ReviewTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsCommentProvided_CommentIsIncludedInReview_ReturnsTrue()
        {
            Review review = new Review()
            {
                Comment = "comment"
            };

            bool result = review.IsCommentProvided();

            Assert.True(result);
        }

        [Test]
        public void IsCommentProvided_CommentIsNotIncludedInReview_ReturnsFalse()
        {
            Review review = new Review();

            bool result = review.IsCommentProvided();

            Assert.False(result);
        }
    }
}