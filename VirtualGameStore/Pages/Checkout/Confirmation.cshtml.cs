using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Checkout
{
    [Authorize]
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
                .Include(u => u.Orders)
                    .ThenInclude(o => o.ShippingAddress)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.BillingAddress)
                .FirstOrDefaultAsync();
        }

        private void Load(User user)
        {
            ShippingAddress = user.ShippingAddress;
            BillingAddress = user.BillingAddress;
        }

        public async Task<IActionResult> OnGetAsync(int id)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return Page();
            }

            byte[] filebytes = Encoding.ASCII.GetBytes(game.Name);
            string contentType = "text/plain";
            string fileDownloadName = game.GenerateFileName();

            return File(filebytes, contentType, fileDownloadName);
        }
    }
}
