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
    public class DetailsModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
