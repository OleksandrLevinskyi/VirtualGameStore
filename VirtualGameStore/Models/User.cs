using Microsoft.AspNetCore.Identity;

namespace VirtualGameStore.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsEmailMarketingEnabled { get; set; }
        public Gender? Gender { get; set; }
    }
}
