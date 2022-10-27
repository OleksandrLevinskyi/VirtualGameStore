using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Reviews
{
    public class AdminModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public AdminModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> Review { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Review != null)
            {
                Review = await _context.Review
                .Include(r => r.Author)
                .Include(r => r.Game).ToListAsync();
            }
        }
    }
}
