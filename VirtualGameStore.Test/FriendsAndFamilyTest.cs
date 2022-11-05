using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    internal class FriendsAndFamilyTest
    {
        [Test]
        public void IsFriend_GivenUserIsNotAFriend_ReturnsFalse()
        {
            string friendId = Guid.NewGuid().ToString();

            var friend = new User()
            {
                Id = friendId
            };

            var user = new User()
            {
                Friends = new List<User>()
            };

            var isFriend = user.IsFriend(friend);

            Assert.That(isFriend, Is.False);
        }

        [Test]
        public void IsFriend_GivenUserIsAFriend_ReturnsFalse()
        {
            string friendId = Guid.NewGuid().ToString();

            var friend = new User()
            {
                Id = friendId
            };

            var user = new User()
            {
                Friends = new List<User>() { friend }
            };

            var isFriend = user.IsFriend(friend);

            Assert.That(isFriend, Is.True);
        }
    }
}
