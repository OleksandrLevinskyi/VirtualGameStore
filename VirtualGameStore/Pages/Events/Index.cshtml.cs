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
    public class IndexModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public IndexModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Event> Event { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Events != null)
            {
                Event = await _context.Events
                .Include(e => e.Creator).ToListAsync();
            }
        }
    }
}
