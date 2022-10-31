using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Checkout
{
    public class ConfirmationModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public Order Order { get; private set; }
        public Address? ShippingAddress { get; private set; }
        public Address? BillingAddress { get; private set; }

        public ConfirmationModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<User?> GetUser()
        {
            if (User.Identity == null)
            {
                return Task.FromResult<User?>(null);
            }

            return _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.Items)
                        .ThenInclude(i => i.Game)
                .Include(u => u.BillingAddress)
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync();
        }

        private void Load(User user)
        {
            ShippingAddress = user.ShippingAddress;
            BillingAddress = user.BillingAddress;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var order = user.Orders.Where(o => o.Id == id).FirstOrDefault();

            if (order == null)
            {
                return NotFound("Order not found");
            }

            Order = order;

            Load(user);

            return Page();
        }
    }
}
