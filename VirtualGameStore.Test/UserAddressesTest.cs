using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class UserAddressesTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AreAddressesEqual_SameAddresses_ReturnsTrue()
        {
            var address = new Address()
            {
                Id = 1,
                Address1 = "1 King St.",
                City = "Waterloo",
                Province = "ON",
                Country = "Canada"
            };

            var user = new User()
            {
                BillingAddress = address,
                ShippingAddress = address
            };

            var areAddressesEqual = user.AreAddressesEqual;

            Assert.That(areAddressesEqual, Is.True);
        }

        [Test]
        public void AreAddressesEqual_DifferentAddresses_ReturnsFalse()
        {
            var billingAddress = new Address()
            {
                Id = 1,
                Address1 = "1 King St.",
                City = "Waterloo",
                Province = "ON",
                Country = "Canada"
            };
            var shippingAddress = new Address()
            {
                Id = 2,
                Address1 = "2 Queen St.",
                City = "Waterloo",
                Province = "ON",
                Country = "Canada"
            };

            var user = new User()
            {
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress
            };

            var areAddressesEqual = user.AreAddressesEqual;

            Assert.That(areAddressesEqual, Is.False);
        }
    }
}