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
    public class DetailsModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public PaymentOption PaymentOption { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PaymentOptions == null)
            {
                return NotFound();
            }

            var paymentoption = await _context.PaymentOptions.FirstOrDefaultAsync(m => m.Id == id);
            if (paymentoption == null)
            {
                return NotFound();
            }
            else 
            {
                PaymentOption = paymentoption;
            }
            return Page();
        }
    }
}
