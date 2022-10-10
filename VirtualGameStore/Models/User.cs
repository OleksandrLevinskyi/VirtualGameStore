using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        [PersonalData]
        public DateTime? BirthDate { get; set; }

        [PersonalData]
        public bool IsEmailMarketingEnabled { get; set; }

        public Gender? Gender { get; set; }
        public IEnumerable<GamePlatform> FavoritePlatforms { get; set; }

        public List<Event>? Events { get; set; }

        public List<Registration>? Registrations { get; set; }

        public bool IsBirthDateValid()
        {
            if (BirthDate == null)
            {
                return false;
            }

            return BirthDate < DateTime.Now;
        }
    }
}
