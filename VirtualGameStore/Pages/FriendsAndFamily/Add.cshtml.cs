using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.FriendsAndFamily
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AddModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        [DisplayName("Username")]
        public string UserName { get; set; }

        private Task<User?> GetUser()
        {
            if (User.Identity == null)
            {
                return Task.FromResult<User?>(null);
            }

            return _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.Friends)
                .FirstOrDefaultAsync();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            if (!ModelState.IsValid)
            {
                return Page();
            }

            var friend = await _context.Users
                .Where(u => u.NormalizedUserName == UserName.Trim().ToUpper())
                .FirstOrDefaultAsync();

            if (friend == null)
            {
                ModelState.AddModelError(nameof(UserName), "Member not found.");
                return Page();
            }

            if (friend.Id == user.Id)
            {
                ModelState.AddModelError(nameof(UserName), "You cannot add yourself.");
                return Page();
            }

            if (user.Friends.Any(f => f.UserName == UserName))
            {
                ModelState.AddModelError(nameof(UserName), "Member is already in your friends and family list.");
                return Page();
            }

            user.Friends = user.Friends.Append(friend);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
