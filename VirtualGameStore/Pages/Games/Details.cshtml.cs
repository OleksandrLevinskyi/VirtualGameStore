using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        [BindProperty]
        public Review Review { get; set; }

        [BindProperty]
        public Game Game { get; set; }

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, bool? isSuccess, bool? isAddedToWishList)
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

            if (isAddedToWishList == true)
            {
                ViewData["DisplayWishListMessage"] = true;
            }

            ViewData["IsAuthorized"] = User.IsInRole("Member");

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

            return Redirect($"/Game/Details?id={Review.GameId}&isSuccess=true");
        }

        public async Task<IActionResult> OnPostAddToWishListAsync()
        {
            string? currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? currUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == currUserId);
            Game? game = await _context.Games.FindAsync(Game.Id);

            if (currUser == null || game == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            currUser.WishList.Add(game);

            _context.Attach(currUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Redirect($"/Games/Details?id={game.Id}&isAddedToWishList=true");
        }
    }
}
