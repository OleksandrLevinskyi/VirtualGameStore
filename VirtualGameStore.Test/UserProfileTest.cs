using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class UserProfileTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsBirthDateValid_BirthDateIsBeforeToday_ReturnsTrue()
        {
            var user = new User()
            {
                BirthDate = DateTime.Now.AddDays(-1)
            };

            var isBirthDateValid = user.IsBirthDateValid();

            Assert.That(isBirthDateValid, Is.True);
        }

        [Test]
        public void IsBirthDateValid_BirthDateIsAfterToday_ReturnsFalse()
        {
            var user = new User()
            {
                BirthDate = DateTime.Now.AddDays(1)
            };

            var isBirthDateValid = user.IsBirthDateValid();

            Assert.That(isBirthDateValid, Is.False);
        }
    }
}