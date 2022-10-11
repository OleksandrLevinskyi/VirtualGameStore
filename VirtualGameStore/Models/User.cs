﻿using Microsoft.AspNetCore.Identity;

namespace VirtualGameStore.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }
        [PersonalData]
        public string? LastName { get; set; }
        [PersonalData]
        public DateTime? BirthDate { get; set; }
        [PersonalData]
        public bool IsEmailMarketingEnabled { get; set; }

        public Gender? Gender { get; set; }

        public bool IsBirthDateValid()
        {
            if (BirthDate == null)
            {
                return false;
            }

            return BirthDate < DateTime.Now;
        }
    }
}