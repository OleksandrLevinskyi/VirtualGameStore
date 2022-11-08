using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    internal class CartTest
    {
        [Test]
        public void GetCartItem_UserDoesNotHaveTheItem_ReturnsNull()
        {
            var user = new User()
            {
                CartItems = Enumerable.Empty<CartItem>()
            };

            var cartItem = user.GetCartItem(1);

            Assert.That(cartItem, Is.Null);
        }

        [Test]
        public void GetCartItem_UserHasTheItem_ReturnsNull()
        {
            var game = new Game()
            {
                Id = 1
            };

            var userId = Guid.NewGuid().ToString();

            var user = new User()
            {
                Id = userId,
                CartItems = new List<CartItem>()
                {
                    new CartItem()
                    {
                        UserId = userId,
                        GameId = game.Id,
                    }
                }
            };

            var cartItem = user.GetCartItem(1);

            Assert.That(cartItem, Is.Not.Null);
        }
    }
}
