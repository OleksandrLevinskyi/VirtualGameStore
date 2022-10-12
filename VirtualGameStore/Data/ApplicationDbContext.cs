using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Models;

namespace VirtualGameStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PaymentOption>()
                .HasOne(x => x.User)
                .WithMany(x => x.PaymentOptions)
                .HasForeignKey(x => x.UserId);
            
            builder.Entity<Event>()
                .HasOne<User>(e => e.Creator)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.CreatorId);

            builder.Entity<Registration>()
                .HasOne<User>(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId);

            builder.Entity<Registration>()
                .HasOne<Event>(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId);
        }
    }
}