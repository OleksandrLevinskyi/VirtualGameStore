using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Profile.ManagePaymentOptions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            User? user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var key = $"{nameof(PaymentOption)}.{nameof(PaymentOption.User)}";
                PaymentOption.User = user;
                ModelState.ClearValidationState(key);
                ModelState.MarkFieldSkipped(key);
            }
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
