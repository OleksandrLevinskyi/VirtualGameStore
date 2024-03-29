﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VirtualGameStore.Models.ValidationAttributes;

namespace VirtualGameStore.Models
{
    public class PaymentOption
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }

        // We can do `[CreditCard]` but then it has to truly be valid
        [DisplayName("Card Number")]
        [RegularExpression("^\\d{16}$", ErrorMessage = "{0} must be a 16 digit number.")]
        public string CardNumber { get; set; }

        [DisplayName("Expiration Date")]
        [ValidFutureMonthYear]
        public string ExpiryDate { get; set; }

        [DisplayName("First Name")]
        public string HolderFirstName { get; set; }

        [DisplayName("Last Name")]
        public string HolderLastName { get; set; }
    }
}