using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Profile.ManagePaymentOptions
{
    public class IndexModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public IndexModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PaymentOption> PaymentOption { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PaymentOptions != null)
            {
                PaymentOption = await _context.PaymentOptions.ToListAsync();
            }
        }
    }
}
