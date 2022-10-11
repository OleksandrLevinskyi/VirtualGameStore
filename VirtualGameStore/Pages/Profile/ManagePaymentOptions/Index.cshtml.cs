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
    }
}
