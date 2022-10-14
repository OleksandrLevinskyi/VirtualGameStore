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
                FavoriteCategories = new List<Category>()
                {
                    new Category() { Id = 1, Name = "Racing" },
                    new Category() { Id = 2, Name = "RPG" }
                },
                FavoritePlatforms = new List<Platform>()
                {
                    new Platform() { Id = 1, Name = "Xbox" },
                    new Platform() { Id = 2, Name = "PC" }
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
                FavoriteCategories = new List<Category>(),
                FavoritePlatforms = new List<Platform>()
            };

            var arePreferencesEmpty = user.ArePreferencesEmpty;

            Assert.That(arePreferencesEmpty, Is.True);
        }

        [Test]
        public void ArePreferencesEmpty_PlatformsAndCategoriesAreNotEmpty_ReturnsFalse()
        {
            var user = new User()
            {
                FavoriteCategories = new List<Category>()
                {
                    new Category() { Id = 1, Name = "Racing" },
                    new Category() { Id = 2, Name = "RPG" }
                },
                FavoritePlatforms = new List<Platform>()
                {
                    new Platform() { Id = 1, Name = "Xbox" },
                    new Platform() { Id = 2, Name = "PC" }
                }
            };

            var arePreferencesEmpty = user.ArePreferencesEmpty;

            Assert.That(arePreferencesEmpty, Is.False);
        }
    }
}