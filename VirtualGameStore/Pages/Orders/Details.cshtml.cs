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
    public class DetailsModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<Order?> GetOrderAsync(int? id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(u => u.BillingAddress)
                .Include(u => u.ShippingAddress)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var order = await GetOrderAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            Order = order;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var order = await GetOrderAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.IsProcessed = true;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
