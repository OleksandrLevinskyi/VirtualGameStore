using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Models;

namespace VirtualGameStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // FriendsFamilyListEntry
            builder.Entity<FriendsFamilyListEntry>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.FriendsEntries);
            builder.Entity<FriendsFamilyListEntry>()
                .HasOne(x => x.Friend)
                .WithMany(x => x.FriendsOfEntries);

                
            // PaymentOption
            builder.Entity<PaymentOption>()
                .HasOne(x => x.User)
                .WithMany(x => x.PaymentOptions);
            // Event
            builder.Entity<Event>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.EventsCreated);
            // Registration
            builder.Entity<Registration>()
                .HasOne(x => x.Event)
                .WithMany(x => x.RegisteredUsers);
            builder.Entity<Registration>()
                .HasOne(x => x.User)
                .WithMany(x => x.EventRegistrations);
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<PaymentOption> PaymentOption { get; set; }
    }
}