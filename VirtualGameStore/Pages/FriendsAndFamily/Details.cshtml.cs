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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public Friendship Friendship { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Friendships == null)
            {
                return NotFound();
            }

            var friendship = await _context.Friendships.FirstOrDefaultAsync(m => m.FriendId == id);
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
    }
}
