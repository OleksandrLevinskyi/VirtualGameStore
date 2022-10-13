using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public CreateModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<Platform> Platforms { get; set; }
        public List<Category> Categories { get; set; }

        public class InputModel
        {
            public List<int> PlatformIds { get; set; } = new List<int>();
            public List<int> CategoryIds { get; set; } = new List<int>();
        }
        private async Task LoadAsync()
        {
            Platforms = await _context.Platforms.OrderBy(g => g.Name).AsNoTracking().ToListAsync();
            Categories = await _context.Categories.OrderBy(g => g.Name).AsNoTracking().ToListAsync();

            Input = new InputModel();
        }

        public async Task<IActionResult> OnGet()
        {
            await LoadAsync();

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
                await LoadAsync();

                ViewData["StatusMessage"] = "Game must have at least one category and platform selected.";

                return Page();
            }

            _context.Games.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
