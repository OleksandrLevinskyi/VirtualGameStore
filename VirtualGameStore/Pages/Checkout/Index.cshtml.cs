using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public int CreditCartId { get; set; }
        public List<PaymentOption> CreditCards { get; set; }
        public List<CartItem> CartItems { get; set; }
        public double Total { get => CartItems.Aggregate(0.0, (total, cartItem) => total + cartItem.Game.Price * cartItem.Quantity); }

        private Task<User?> GetUser()
        {
            if (User.Identity == null)
            {
                return Task.FromResult<User?>(null);
            }

            return _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.CartItems)
                    .ThenInclude(i => i.Game)
                .Include(u => u.PaymentOptions)
                .FirstOrDefaultAsync();
        }

        private async Task LoadAsync(User user)
        {
            CartItems = user.CartItems.ToList();
            CreditCards = user.PaymentOptions.ToList();
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }
    }
}
