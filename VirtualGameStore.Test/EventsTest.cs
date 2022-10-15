using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class EventsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsOverAttendeeLimit_ReturnsTrueIfAttendeeLimitIsOverTotalRegistrations()
        {
            Event newEvent = new Event()
            {
                AttendeeLimit = 1,
                Registrations = new List<Registration> { 
                    new Registration(),
                    new Registration(),
                    new Registration()
                }
            };

            bool isOverAttendeeLimit = newEvent.IsOverAttendeeLimit();

            Assert.True(isOverAttendeeLimit);
        }

        [Test]
        public void IsOverAttendeeLimit_ReturnsFalseIfAttendeeLimitIsLessThanTotalRegistrations()
        {
            Event newEvent = new Event()
            {
                AttendeeLimit = 3,
                Registrations = new List<Registration> {
                    new Registration(),
                    new Registration()
                }
            };

            bool isOverAttendeeLimit = newEvent.IsOverAttendeeLimit();

            Assert.False(isOverAttendeeLimit);
        }

        [Test]
        public void IsInFuture_ReturnsTrueIfEventDateTimeIsAfterNow()
        {
            Event newEvent = new Event()
            {
                DateTime = DateTime.Now.AddDays(1)
            };

            bool isNewEventInFuture = newEvent.IsInFuture();

            Assert.True(isNewEventInFuture);
        }

        [Test]
        public void IsInFuture_ReturnsFalseIfEventDateTimeIsBeforeNow()
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