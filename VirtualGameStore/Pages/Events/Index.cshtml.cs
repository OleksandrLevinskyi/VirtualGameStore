using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public IndexModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int EventId { get; set; }
        public IList<Event> Events { get; set; } = default!;

        public async Task OnGetAsync(bool? isSuccess)
        {
            string? currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (isSuccess == true)
            {
                ViewData["StatusMessage"] = "You have successfully registered for the event.";
            }
            else if (isSuccess == false)
            {
                ViewData["ErrorMessage"] = "Something went wrong. Please try again.";
            }

            if (_context.Events != null)
            {
                Events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Registrations)
                .ToListAsync();

                Events = Events.Where(e => !e.IsOverAttendeeLimit() || e.Registrations.Any(r => r.UserId == currUserId)).ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string? currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Event? registrationEvent = await _context.Events
                .Include(r => r.Registrations)
                .FirstOrDefaultAsync(e => e.Id == EventId);

            if (currUserId == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            if (registrationEvent == null || registrationEvent.IsOverAttendeeLimit())
            {
                return Redirect("/Events/Index?isSuccess=false");
            }

            Registration registration = new Registration()
            {
                UserId = currUserId,
                EventId = EventId
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return Redirect("/Events/Index?isSuccess=true");
        }
    }
}
