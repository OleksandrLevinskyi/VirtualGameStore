﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Reviews
{
    public class DetailsModel : PageModel
    {
        private readonly VirtualGameStore.Data.ApplicationDbContext _context;

        public DetailsModel(VirtualGameStore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            else 
            {
                Review = review;
            }
            return Page();
        }
    }
}
