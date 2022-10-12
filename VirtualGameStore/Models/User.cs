using Microsoft.AspNetCore.Identity;

namespace VirtualGameStore.Models
{
    public class User : IdentityUser
    {
        public virtual IEnumerable<PaymentOption> PaymentOptions { get; }
    }
}