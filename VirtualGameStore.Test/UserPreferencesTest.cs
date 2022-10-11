using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class UserPreferencesTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ResetPreferences_EmptiesPlatformsAndCategories()
        {
            var user = new User()
            {
                FavoriteCategories = new List<GameCategory>()
                {
                    new GameCategory() { Id = 1, Name = "Racing" },
                    new GameCategory() { Id = 2, Name = "RPG" }
                },
                FavoritePlatforms = new List<GamePlatform>()
                {
                    new GamePlatform() { Id = 1, Name = "Xbox" },
                    new GamePlatform() { Id = 2, Name = "PC" }
                }
            };

            user.ResetPreferences();

            Assert.That(user.FavoriteCategories, Is.Empty);
            Assert.That(user.FavoritePlatforms, Is.Empty);
        }

        [Test]
        public void ArePreferencesEmpty_PlatformsAndCategoriesAreEmpty_ReturnsTrue()
        {
            var user = new User()
            {
                FavoriteCategories = new List<GameCategory>(),
                FavoritePlatforms = new List<GamePlatform>()
            };

            var arePreferencesEmpty = user.ArePreferencesEmpty;

            Assert.That(arePreferencesEmpty, Is.True);
        }

        [Test]
        public void ArePreferencesEmpty_PlatformsAndCategoriesAreNotEmpty_ReturnsFalse()
        {
            var user = new User()
            {
                FavoriteCategories = new List<GameCategory>()
                {
                    new GameCategory() { Id = 1, Name = "Racing" },
                    new GameCategory() { Id = 2, Name = "RPG" }
                },
                FavoritePlatforms = new List<GamePlatform>()
                {
                    new GamePlatform() { Id = 1, Name = "Xbox" },
                    new GamePlatform() { Id = 2, Name = "PC" }
                }
            };

            var arePreferencesEmpty = user.ArePreferencesEmpty;

            Assert.That(arePreferencesEmpty, Is.False);
        }
    }
}