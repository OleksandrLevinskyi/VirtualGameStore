using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(VirtualGameStore.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IList<CartItemInput> CartItemInputs { get; set; }
        public IList<SelectList> ItemInputSelectLists { get; set; }
        public IList<CartItem> CartItems { get;set; } = default!;

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
                .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CartItems = user.CartItems.ToList();

            CartItemInputs = CartItems.Select(i => new CartItemInput()
            {
                GameId = i.GameId,
                Quantity = i.Quantity,
                GameName = i.Game.Name,
                Price = (decimal) i.Game.Price
            }).ToList();

            ItemInputSelectLists = CartItems.Select(i =>
            {
                var stock = i.Game.IsDigital ? 1 : Math.Min(i.Game.Stock, 10);

                return new SelectList(Enumerable.Range(1, stock), i.Quantity);
            }).ToList();

            return Page();
        }

        public class CartItemInput
        {
            public string GameName { get; set; }
            public decimal Price { get; set; }
            public bool Remove { get; set; }
            public int GameId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
