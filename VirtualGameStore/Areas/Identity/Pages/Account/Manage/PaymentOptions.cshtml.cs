using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Profile.ManagePaymentOptions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<PaymentOption> PaymentOption { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PaymentOptions != null)
            {
                var user = await _userManager.GetUserAsync(User);
                PaymentOption = await _context.PaymentOptions
                    .Where(p => p.User.Id == user.Id)
                    .ToListAsync();
            }
        }
        
        public async Task<IActionResult> OnPostAsync(bool delete, int? id)
        {
            if (id == null || _context.PaymentOptions == null || delete == false)
            {
                return NotFound();
            }
            var paymentOption = await _context.PaymentOptions.FindAsync(id);

            User? user = await _userManager.GetUserAsync(User);

            if (paymentOption is not null)
            {
                // Make sure the logged in user is the owner of the credit card
                if (user.Id != paymentOption.UserId)
                {
                    return NotFound();
                }
                else
                {
                    _context.PaymentOptions.Remove(paymentOption);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("PaymentOptions");
        }
    }
}
