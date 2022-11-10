using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class RegistrationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsOverAttendeeLimit_AttendeeLimitIsOverTotalRegistrations_ReturnTrue()
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
        public void IsOverAttendeeLimit_AttendeeLimitIsLessThanTotalRegistrations_ReturnFalse()
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
    }
}