using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VirtualGameStore.Data;
using VirtualGameStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace VirtualGameStore.Pages.Events
{
    [Authorize(Roles = "Employee")]
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
        public Event Event { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Event.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Admin");
        }
    }
}
