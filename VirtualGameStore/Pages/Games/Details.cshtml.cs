using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Review Review { get; set; }

        public Game Game { get; set; }
        public SelectList CartItemQuantitySelectList { get; private set; }

        private Task<User?> GetUser()
        {
            if (User.Identity == null)
            {
                return Task.FromResult<User?>(null);
            }

            return _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> OnGetAsync(int? id, bool? isSuccess)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.Categories)
                .Include(
                    g => g.Reviews
                    .Where(r => r.IsApproved == true)
                    .OrderByDescending(r => r.Rating)
                )
                .ThenInclude(r => r.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            Review = new Review() { GameId = game.Id, AuthorId = "none" };

            if (game == null)
            {
                return NotFound();
            }
            else
            {
                Game = game;
            }

            if (isSuccess == true)
            {
                ViewData["IsSuccess"] = true;
            }

            ViewData["IsAuthorized"] = User.IsInRole("Member");

            var stock = game.IsDigital ? 1 : Math.Min(game.Stock, 10);
            CartItemQuantitySelectList = new SelectList(Enumerable.Range(1, stock), 1);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Review.Comment = (Review.Comment + "").Trim();
            Review.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Review.DateTime = DateTime.Now;

            if (!ModelState.IsValid || !User.IsInRole("Member"))
            {
                return Page();
            }

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return Redirect($"./Details?id={Review.GameId}&isSuccess=true");
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int? id, int? quantity)
        {
            if (id == null || quantity == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var cartItem = user.GetCartItem(id);

            // TODO: if game is digital and user has already purchased it,
            // show message saying it cannot be added to cart and can be downloaded
            // in the dowloads section
            // ALTERNATIVELY: disable "add to cart button" with a message underneath it

            if (cartItem == null)
            {
                user.CartItems = user.CartItems.Append(new CartItem()
                {
                    User = user,
                    Game = game,
                    Quantity = quantity ?? 1
                }).ToList();

                TempData["SuccessMessage"] = "Game successfully added to your cart.";
                await _context.SaveChangesAsync();
                return RedirectToPage(new { game.Id });
            }

            if (!game.IsDigital)
            {
                cartItem.Quantity += quantity ?? 1;
                // TODO: Decrease stock
            }
            else
            {
                TempData["ErrorMessage"] = "You can only add a digital game to your cart one.";
                return RedirectToPage(new { game.Id });
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Game successfully added to your cart.";
            return RedirectToPage(new { game.Id });
        }
    }
}
