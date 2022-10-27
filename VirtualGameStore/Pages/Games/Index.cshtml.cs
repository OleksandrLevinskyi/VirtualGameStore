using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GameCount;
        public IList<Game> Games { get; set; } = default!;
        public List<Platform> Platforms { get; set; }
        public List<Category> Categories { get; set; }
        
        [BindProperty]
        public SearchInputModel? Search { get; set; }

        public async Task OnGetAsync()
        {
            Platforms = await _context.Platforms.OrderBy(g => g.Name).AsNoTracking().ToListAsync();
            Categories = await _context.Categories.OrderBy(g => g.Name).AsNoTracking().ToListAsync();
            Games = await _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.Categories)
                .ToListAsync();
            GameCount = Games.Count;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Platforms = await _context.Platforms.OrderBy(g => g.Name).AsNoTracking().ToListAsync();
            Categories = await _context.Categories.OrderBy(g => g.Name).AsNoTracking().ToListAsync();
            
            var query =  _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.Categories)
                .AsQueryable();
            
            if (Search!.TextSearch is not null)
                query = query.Where(g => g.Name.Contains(Search!.TextSearch!));

            if (Search!.MaxPrice is not null)
                query = query.Where(g => g.Price < Search!.MaxPrice);

            if (Search!.Medium is not null)
                query = query.Where(g => g.IsDigital == Search!.Medium);

            if (Search!.PlatformIds.Any())
                query = query.Where(g => 
                    g.Platforms!.Count(p => Search!.PlatformIds.Contains(p.Id)) >= Search!.PlatformIds.Count
                );

            if (Search!.CategoryIds.Any())
                query = query.Where(g => 
                    g.Categories!.Count(c => Search!.CategoryIds.Contains(c.Id)) >= Search!.CategoryIds.Count
                );
            
            Games = await query.ToListAsync();
            GameCount = await _context.Games.CountAsync();
            return Page();
        }

        public class SearchInputModel
        {
            public string? TextSearch { get; init; }
            public float? MaxPrice { get; init; }
            public bool? Medium { get; init; }
            public List<int> PlatformIds { get; init; } = new();
            public List<int> CategoryIds { get; init; } = new();
        }

    }
}
