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

        public async Task OnGetAsync(string? message)
        {
            string? currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (message == "add")
            {
                ViewData["StatusMessage"] = "You have successfully registered for the event.";
            }
            else if (message == "remove")
            {
                ViewData["StatusMessage"] = "You have successfully deregistered from the event.";
            }
            else if (message == "fail")
            {
                ViewData["ErrorMessage"] = "Something went wrong. Please try again.";
            }

            if (_context.Events != null)
            {
                Events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Registrations)
                .ToListAsync();

                Events = Events.Where(e => (!e.IsOverAttendeeLimit() || e.Registrations.Any(r => r.UserId == currUserId)) && e.IsInFuture()).ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string message = "fail";
            string? currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Event? registrationEvent = await _context.Events
                .Include(r => r.Registrations)
                .FirstOrDefaultAsync(e => e.Id == EventId);

            if (currUserId == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            if (registrationEvent == null)
            {
                return Redirect($"/Events/Index?message={message}");
            }

            Registration? userRegisteration = registrationEvent.Registrations.FirstOrDefault(r => r.UserId == currUserId);

            if(registrationEvent.IsOverAttendeeLimit() && userRegisteration == null)
            {
                return Redirect($"/Events/Index?message={message}");
            }

            if (userRegisteration != null)
            {
                message = "remove";
                _context.Registrations.Remove(userRegisteration);
            }
            else
            {
                Registration registration = new Registration()
                {
                    UserId = currUserId,
                    EventId = EventId
                };

                message = "add";
                _context.Registrations.Add(registration);
            }

            await _context.SaveChangesAsync();

            return Redirect($"/Events/Index?message={message}");
        }
    }
}
