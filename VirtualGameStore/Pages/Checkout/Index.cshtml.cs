using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Checkout
{
    [Authorize]
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
        public Input InputModel { get; set; }
        public List<PaymentOption> CreditCards { get; set; }
        public List<CartItem> CartItems { get; set; }
        public double Total { get => CartItems.Aggregate(0.0, (total, cartItem) => total + cartItem.Game.Price * cartItem.Quantity); }
        public Address? ShippingAddress { get; set; }
        public Address? BillingAddress { get; set; }

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
                .Include(u => u.BillingAddress)
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync();
        }

        private void Load(User user)
        {
            CartItems = user.CartItems.ToList();
            CreditCards = user.PaymentOptions.ToList();
            ShippingAddress = user.ShippingAddress;
            BillingAddress = user.BillingAddress;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            var orderItems = user.CartItems.Select(cartItem => new OrderItem()
            {
                Game = cartItem.Game,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Game.Price,
            }).ToList();

            var order = new Order()
            {
                User = user,
                Items = orderItems,
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            // TODO: clear cart
            await _context.SaveChangesAsync();
            return RedirectToPage("/Checkout/Confirmation", new { id = order.Id });
        }

        public class Input
        {
            [Required(ErrorMessage = "You must choose a credit card.")]
            public int CreditCardId { get; set; }
        }
    }
}
