using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IList<Review> Reviews { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Review != null)
            {
                Reviews = await _context.Review
                .Include(r => r.Author)
                .Include(r => r.Game)
                .Where(r => r.IsApproved == null)
                .OrderBy(r => r.DateTime)
                .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id, bool? isApproved)
        {
            if (id == null || isApproved == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            review.IsApproved = (bool)isApproved;

            _context.Attach(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToPage("./Admin");
        }
    }
}
