using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.WishList
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public User WishListOwner { get; set; }
        public List<Game> WishList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            User? currUser = await _context.Users.Include(u => u.WishList).FirstOrDefaultAsync(u => u.Id == id);

            if (currUser == null)
            {
                return NotFound();
            }

            WishListOwner = currUser;
            WishList = currUser.WishList;

            return Page();
        }
    }
}
