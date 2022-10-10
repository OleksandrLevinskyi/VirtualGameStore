using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Areas.Identity.Pages.Account.Manage
{
    public class PreferencesModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public PreferencesModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<GamePlatform> GamePlatforms { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IEnumerable<int> FavoritePlatformIds { get; set; }
        }

        private async Task<User?> GetUser()
        {
            if (User.Identity == null)
            {
                return null;
            }

            return await _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.FavoritePlatforms)
                .FirstOrDefaultAsync();
        }

        private async Task LoadAsync(User user)
        {
            GamePlatforms = await _context.GamePlatforms.OrderBy(g => g.Name).AsNoTracking().ToListAsync();


            Input = new InputModel
            {
                FavoritePlatformIds = user.FavoritePlatforms
                .ToList()
                .Select(p => p.Id)
            };
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await GetUser();

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

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


            user.FavoritePlatforms = await _context.GamePlatforms
                .Where(p => Input.FavoritePlatformIds.Contains(p.Id))
                .ToListAsync();

            await _userManager.UpdateAsync(user);

            StatusMessage = "Your preferences have been updated";
            return RedirectToPage();
        }
    }
}
