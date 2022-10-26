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

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; }

        public Game Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Reviews.OrderByDescending(r => r.DateTime))
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

            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return Redirect("./Details" + "?id=" + Review.GameId);
        }
    }
}
