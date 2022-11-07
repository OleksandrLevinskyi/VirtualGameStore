using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public DetailsModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            if (messageType == "wishlist-success")
            {
                ViewData["DisplayWishListMessage"] = true;
            }

            ViewData["IsGameAlreadyInWishList"] = false;
            ViewData["IsAuthorized"] = User.IsInRole("Member");

            string currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? currUser = await _context.Users.Include(u => u.WishList).FirstOrDefaultAsync(u => u.Id == currUserId);

            if (currUser != null)
            {
                ViewData["IsGameAlreadyInWishList"] = currUser.WishList.Any(g => g.Id == Game.Id);
            }

            return Page();
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

            bool isGameAlreadyInWishList = currUser.WishList.Any(g => g.Id == game.Id);

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

            return Redirect($"/Games/Details?id={game.Id}&messageType=wishlist-success");
        }
    }
}
