using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsEmailMarketingEnabled { get; set; }
        public Gender? Gender { get; set; }
        public Address? BillingAddress { get; set; }
        public Address? ShippingAddress { get; set; }
        public IEnumerable<Platform> FavoritePlatforms { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Event>? Events { get; set; }
        public List<Registration>? Registrations { get; set; }
        public List<Game> WishList { get; set; } = new List<Game>();
        public IEnumerable<Category> FavoriteCategories { get; set; }
        public virtual IEnumerable<PaymentOption> PaymentOptions { get; }

        // TODO: Does the other user need to accept the friendship?
        public IEnumerable<User> Friends { get; set; }
        public IEnumerable<User> FriendOf { get; set; }

        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
        public IEnumerable<Order> Orders { get; set; }

        public bool AreAddressesEqual { get => BillingAddress?.Id == ShippingAddress?.Id; }
        public bool ArePreferencesEmpty
        {
            get => !FavoritePlatforms.Any() || !FavoriteCategories.Any();
        }

        public bool IsGameInWishList(int gameId)
        {
            return WishList.Any(g => g.Id == gameId);
        }

        public bool IsGamePurchased(int gameId)
        {
            return Orders.Any(o => o.Items.Any(i => i.Game.Id == gameId));
        }

        public bool IsBirthDateValid()
        {
            if (BirthDate == null)
            {
                return false;
            }

            return BirthDate < DateTime.Now;
        }

        public void ResetPreferences()
        {
            FavoriteCategories = new List<Category>();
            FavoritePlatforms = new List<Platform>();
        }

        public CartItem? GetCartItem(int? gameId)
        {
            return CartItems.Where(i => i.GameId == gameId).FirstOrDefault();
        }

        public bool IsFriend(User user)
        {
            return Friends.Any(f => f.Id == user.Id);
        }
    }
}