using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Orders
{
    [Authorize(Roles = "Employee")]
    public class IndexModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public IndexModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Orders = await _context.Orders
                    .Where(o => !o.IsProcessed)
                    .Include(o => o.BillingAddress)
                    .Include(o => o.ShippingAddress)
                    .Include(o => o.User)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Game)
                    .ToListAsync();
            }
        }
    }
}
