using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Games
{
    [Authorize(Roles = "Employee")]
    public class EditModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public EditModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }
        public List<Platform> Platforms { get; set; }
        public List<Category> Categories { get; set; }

        public class InputModel
        {
            public List<int> PlatformIds { get; set; } = new List<int>();
            public List<int> CategoryIds { get; set; } = new List<int>();
        }

        private async Task LoadAsync(Game game)
        {
            Platforms = await _context.Platforms.OrderBy(g => g.Name).AsNoTracking().ToListAsync();
            Categories = await _context.Categories.OrderBy(g => g.Name).AsNoTracking().ToListAsync();

            Input = new InputModel()
            {
                PlatformIds = game.Platforms.Select(p => p.Id).ToList(),
                CategoryIds = game.Categories.Select(p => p.Id).ToList()
            };
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g =>g.Platforms)
                .Include(g =>g.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            Game = game;

            await LoadAsync(Game);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Game.Platforms = await _context.Platforms
                  .Where(p => Input.PlatformIds.Contains(p.Id))
                  .ToListAsync();

            Game.Categories = await _context.Categories
                .Where(c => Input.CategoryIds.Contains(c.Id))
                .ToListAsync();

            if (
                !ModelState.IsValid ||
                Game.Categories.Count == 0 ||
                Game.Platforms.Count == 0
                )
            {
                await LoadAsync(Game);

                ViewData["StatusMessage"] = "Game must have at least one category and platform selected.";

                return Page();
            }

            _context.Attach(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(Game.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
