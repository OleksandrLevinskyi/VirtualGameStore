using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class User : IdentityUser
    {
        public List<Event>? Events { get; set; }
        public List<Registration>? Registrations { get; set; }
    }
}
