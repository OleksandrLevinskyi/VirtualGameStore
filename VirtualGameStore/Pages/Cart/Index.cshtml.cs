using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Cart
{
    [Authorize]
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
        public IList<CartItemCard> CartItemCards { get; set; }
        public IList<SelectList> CartItemQuantitySelectLists { get; set; }
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

            CartItemCards = CartItems.Select(i => new CartItemCard()
            {
                GameId = i.GameId,
                Quantity = i.Quantity,
                GameName = i.Game.Name,
                Price = (decimal) i.Game.Price
            }).ToList();

            CartItemQuantitySelectLists = CartItems.Select(i =>
            {
                var stock = i.Game.IsDigital ? 1 : Math.Min(i.Game.Stock, 10);

                return new SelectList(Enumerable.Range(1, stock), i.Quantity);
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.CartItems = CartItemCards.Aggregate(new List<CartItem>(), (list, itemCard) =>
            {
                if (itemCard.Remove)
                {
                    return list;
                }

                var cartItem = user.GetCartItem(itemCard.GameId);

                if (cartItem == null)
                {
                    return list;
                }

                cartItem.Quantity = itemCard.Quantity;

                return list.Append(cartItem).ToList();
            });

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public class CartItemCard
        {
            public string GameName { get; set; }
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }
            public bool Remove { get; set; }
            public int GameId { get; set; }
            public int Quantity { get; set; }

            [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
            public decimal Total { get => Price * Quantity; }
        }
    }
}
