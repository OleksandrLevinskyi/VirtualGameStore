using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Profile.WishList
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Game> WishList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            string currUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? currUser = await _context.Users.Include(u => u.WishList).FirstOrDefaultAsync(u => u.Id == currUserId);

            if (currUser != null)
            {
                string wishListUrl = $"https://{Request.Host}/WishList/Index?id={currUser.Id}";
                string twitterLink = $"https://twitter.com/intent/tweet?text=Take%20a%20look%20at%20my%20Game%20Store%20Wishlist:&url={wishListUrl}";

                ViewData["TwitterLink"] = twitterLink;

                WishList = currUser.WishList;
            }
        }
    }
}
