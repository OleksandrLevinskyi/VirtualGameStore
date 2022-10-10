using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Profile.ManagePaymentOptions
{
    public class CreateModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public CreateModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PaymentOption PaymentOption { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.PaymentOptions == null || PaymentOption == null)
            {
                return Page();
            }

            _context.PaymentOptions.Add(PaymentOption);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
