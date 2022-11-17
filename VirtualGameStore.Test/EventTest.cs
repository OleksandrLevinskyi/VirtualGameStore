using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class EventTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsInFuture_EventDateTimeIsAfterNow_ReturnsTrue()
        {
            Event newEvent = new Event()
            {
                DateTime = DateTime.Now.AddDays(1)
            };

            bool isNewEventInFuture = newEvent.IsInFuture();

            Assert.True(isNewEventInFuture);
        }

        [Test]
        public void IsInFuture_EventDateTimeIsBeforeNow_ReturnsFalse()
        {
            Event newEvent = new Event()
            {
                DateTime = DateTime.Now.AddDays(-1)
            };

            bool isNewEventInFuture = newEvent.IsInFuture();

            Assert.False(isNewEventInFuture);
        }
    }
}