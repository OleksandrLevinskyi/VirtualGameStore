using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public Review Review { get; set; }
        [BindProperty]
        public int GameId { get; set; }
        public Game Game { get; set; }
        public SelectList CartItemQuantitySelectList { get; private set; }

        public DetailsModel(ApplicationDbContext context, UserManager<User> userManager)
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
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> OnGetAsync(int? id, string? messageType)
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

            if (messageType == "review-success")
            {
                ViewData["DisplayReviewMessage"] = true;
            }

            ViewData["IsGameAlreadyInWishList"] = false;
            ViewData["IsAuthorized"] = User.IsInRole("Member");
            ViewData["IsGamePurchased"] = false;

            string currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? currUser = await _context.Users
                .Include(u => u.WishList)
                .Include(u => u.Orders)
                .ThenInclude(o => o.Items)
                .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(u => u.Id == currUserId);

            if (currUser != null)
            {
                ViewData["IsGameAlreadyInWishList"] = currUser.IsGameInWishList(Game.Id);
                ViewData["IsGamePurchased"] = currUser.IsGamePurchased(Game.Id);
            }

            var stock = game.IsDigital ? 1 : Math.Min(game.Stock, 10);
            CartItemQuantitySelectList = new SelectList(Enumerable.Range(1, stock), 1);

            return Page();
        }

        public async Task<IActionResult> OnPostDownloadAsync()
        {
            Game? game = await _context.Games.FindAsync(GameId);

            if (game == null)
            {
                return Page();
            }

            byte[] filebytes = Encoding.ASCII.GetBytes(game.Name);
            string contentType = "text/plain";
            string fileDownloadName = $"{game.Name.Replace(' ', '_')}.txt";

            return File(filebytes, contentType, fileDownloadName);
        }

        public async Task<IActionResult> OnPostAddReviewAsync()
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

            return Redirect($"/Games/Details?id={Review.GameId}&messageType=review-success");
        }

        public async Task<IActionResult> OnPostAddToWishListAsync()
        {
            string currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? currUser = await _context.Users.Include(u => u.WishList).FirstOrDefaultAsync(u => u.Id == currUserId);
            Game? game = await _context.Games.FindAsync(GameId);

            if (currUser == null || game == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            bool isGameAlreadyInWishList = currUser.IsGameInWishList(GameId);

            if (isGameAlreadyInWishList)
            {
                currUser.WishList.Remove(game);
            }
            else
            {
                currUser.WishList.Add(game);
            }

            _context.Attach(currUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Redirect($"/Games/Details?id={game.Id}");
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
                return Redirect("/Identity/Account/Login");
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

                game.Stock -= quantity ?? 1;

                TempData["SuccessMessage"] = "Game successfully added to your cart.";
                await _context.SaveChangesAsync();
                return RedirectToPage(new { game.Id });
            }

            if (!game.IsDigital)
            {
                cartItem.Quantity += quantity ?? 1;
                game.Stock -= quantity ?? 1;
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
