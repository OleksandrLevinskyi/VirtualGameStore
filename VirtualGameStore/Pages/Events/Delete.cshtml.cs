using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Events
{
    public class DeleteModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public DeleteModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var foundEvent = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if (foundEvent == null)
            {
                return NotFound();
            }
            else 
            {
                Event = foundEvent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }
            var foundEvent = await _context.Events.FindAsync(id);

            if (foundEvent != null)
            {
                Event = foundEvent;
                _context.Events.Remove(Event);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
