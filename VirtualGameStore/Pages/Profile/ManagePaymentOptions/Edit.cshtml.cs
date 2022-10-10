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

namespace VirtualGameStore.Pages.Profile.ManagePaymentOptions
{
    public class EditModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public EditModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaymentOption PaymentOption { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PaymentOptions == null)
            {
                return NotFound();
            }

            var paymentoption =  await _context.PaymentOptions.FirstOrDefaultAsync(m => m.Id == id);
            if (paymentoption == null)
            {
                return NotFound();
            }
            PaymentOption = paymentoption;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PaymentOption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentOptionExists(PaymentOption.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PaymentOptionExists(int id)
        {
          return (_context.PaymentOptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
