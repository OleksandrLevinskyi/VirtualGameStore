﻿using System.ComponentModel.DataAnnotations;

namespace VirtualGameStore.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public int EventId { get; set; }

        public DateTime DateTimeRegistered { get; set; } = DateTime.Now;

        public User? User { get; set; }

        public Event? Event { get; set; }
    }
}
