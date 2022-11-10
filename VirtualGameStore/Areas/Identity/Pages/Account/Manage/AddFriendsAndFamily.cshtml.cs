using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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

        public IEnumerable<User> AvailableUsers { get; set; } = new List<User>();

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

        private async Task LoadAsync(User user)
        {
            var nonCurrentUsers = await _context.Users.Where(u => u.Id != user.Id).ToListAsync();
            foreach (var nonCurrentUser in nonCurrentUsers)
            {
                var isMember = await _userManager.IsInRoleAsync(nonCurrentUser, "Member");
                var isFriend = user.Friends.Any(f => f.Id == nonCurrentUser.Id);
                if (isMember && !isFriend)
                {
                    AvailableUsers = AvailableUsers.Append(nonCurrentUser);
                }
            }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

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
                await LoadAsync(user);
                return Page();
            }

            var friend = await _context.Users
                .Where(u => u.NormalizedUserName == UserName.Trim().ToUpper())
                .FirstOrDefaultAsync();

            if (friend == null)
            {
                ModelState.AddModelError(nameof(UserName), "Member not found.");
                await LoadAsync(user);
                return Page();
            }

            if (friend.Id == user.Id)
            {
                ModelState.AddModelError(nameof(UserName), "You cannot add yourself.");
                await LoadAsync(user);
                return Page();
            }

            if (user.IsFriend(friend))
            {
                ModelState.AddModelError(nameof(UserName), "Member is already in your friends and family list.");
                await LoadAsync(user);
                return Page();
            }

            user.Friends = user.Friends.Append(friend);

            await _context.SaveChangesAsync();

            return RedirectToPage("./FriendsAndFamilyList");
        }
    }
}
