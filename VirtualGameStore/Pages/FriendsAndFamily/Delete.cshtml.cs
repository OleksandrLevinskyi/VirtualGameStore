using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.FriendsAndFamily
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Friendship Friendship { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Friendship == null)
            {
                return NotFound();
            }

            var friendship = await _context.Friendship.FirstOrDefaultAsync(m => m.FriendId == id);

            if (friendship == null)
            {
                return NotFound();
            }
            else 
            {
                Friendship = friendship;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Friendship == null)
            {
                return NotFound();
            }
            var friendship = await _context.Friendship.FindAsync(id);

            if (friendship != null)
            {
                Friendship = friendship;
                _context.Friendship.Remove(Friendship);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
