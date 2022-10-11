﻿using System;
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
    public class AddressesModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AddressesModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public Address BillingAddress { get; set; }
            public Address ShippingAddress { get; set; }
            [Display(Name = "Use this as my shipping address")]
            public bool AreAddressesEqual { get; set; }
        }

        private async Task<User?> GetUser()
        {
            if (User.Identity == null)
            {
                return null;
            }

            return await _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Include(u => u.BillingAddress)
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync();
        }

        private async Task LoadAsync(User user)
        {
            Input = new InputModel
            {
                BillingAddress = user.BillingAddress ?? new Address(),
                ShippingAddress = user.ShippingAddress == null || user.AreAddressesEqual ? new Address() : user.ShippingAddress,
                AreAddressesEqual = user.AreAddressesEqual
            };

            if (user.AreAddressesEqual)
            {
                Input.ShippingAddress = new Address();
            }

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

            var previousBillingAddress = user.BillingAddress;
            var previousShippingAddress = user.ShippingAddress;

            if (Input.AreAddressesEqual)
            {
                Input.ShippingAddress = Input.BillingAddress;
            }

            user.BillingAddress = Input.BillingAddress;
            user.ShippingAddress = Input.ShippingAddress;

            await _userManager.UpdateAsync(user);

            if (previousBillingAddress != null && previousBillingAddress.Id != user.BillingAddress?.Id)
            {
                _context.Addresses.Remove(previousBillingAddress);
            }

            if (previousShippingAddress != null && previousShippingAddress.Id != user.ShippingAddress?.Id)
            {
                _context.Addresses.Remove(previousShippingAddress);
            }

            await _context.SaveChangesAsync();

            StatusMessage = "Your preferences have been updated";
            return RedirectToPage();
        }
    }
}
