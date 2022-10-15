using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class GamesTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsAvailable_ReturnsTrueIfStockIsGreaterThanZero()
        {
            Game game = new Game()
            {
                Stock = 5
            };

            bool isGameAvailable = game.IsAvailable();

            Assert.True(isGameAvailable);
        }

        [Test]
        public void IsAvailable_ReturnsFalseIfStockIsZero()
        {
            Game game = new Game()
            {
                Stock = 0
            };

            bool isGameAvailable = game.IsAvailable();

            Assert.False(isGameAvailable);
        }
    }
}