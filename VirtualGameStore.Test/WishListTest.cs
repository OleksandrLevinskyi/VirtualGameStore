using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class WishListTest
    {
        [Test]
        public void IsGameInWishList_GameIsInUsersWishList_ReturnsTrue()
        {
            Game game = new Game() { Id = 1 };
            User user = new User()
            {
                WishList = new List<Game>() { game }
            };

            bool isGameInWishList = user.IsGameInWishList(game.Id);

            Assert.True(isGameInWishList);
        }

        [Test]
        public void IsGameInWishList_GameIsNotInUsersWishList_ReturnsFalse()
        {
            Game game = new Game() { Id = 1 };
            User user = new User();

            bool isGameInWishList = user.IsGameInWishList(game.Id);

            Assert.False(isGameInWishList);
        }
    }
}