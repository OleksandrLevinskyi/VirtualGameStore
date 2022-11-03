using Microsoft.AspNetCore.Identity;
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
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }
        public DbSet<Review> Reviews { get; set; }

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

            builder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany(u => u.FriendOf)
                .UsingEntity<Friendship>(
                    friendshipEntity => friendshipEntity.HasOne(f => f.User)
                                                        .WithMany()
                                                        .HasForeignKey(f => f.UserId),
                    friendshipEntity => friendshipEntity.HasOne(f => f.Friend)
                                                        .WithMany()
                                                        .HasForeignKey(f => f.FriendId));

            Seed(builder);
        }

        private static void Seed(ModelBuilder builder)
        {
            var employeeRole = new IdentityRole()
            {
                Id = "afe877ff-cf81-4bff-9d50-66238d3a1b9e",
                Name = "Employee",
                NormalizedName = "EMPLOYEE",
                ConcurrencyStamp = "6010635a-cb51-4a21-bb0a-529ece3facbc"
            };

            var memberRole = new IdentityRole()
            {
                Id = "9a86cc44-771d-426d-b702-c4a4a93c348f",
                Name = "Member",
                NormalizedName = "MEMBER",
                ConcurrencyStamp = "00cce6e2-03f1-42cc-805f-91df7dae668e"
            };

            var employeeUser = new User()
            {
                Id = "5abf56ec-8224-42b1-965d-a11bd8d818c7",
                UserName = "employee",
                NormalizedUserName = "EMPLOYEE",
                Email = "employee@vgs.com",
                NormalizedEmail = "EMPLOYEE@VGS.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", // Password1!
                SecurityStamp = "IOHH3QAG6CBJWWO4LDGAJTSSACV2KNDI",
                ConcurrencyStamp = "5e2e9d4a-d8e2-4200-8e13-ef777407f2ca",
            };

            var memberUser = new User()
            {
                Id = "9a44a14a-47fb-4196-8a45-57fa557fb992",
                UserName = "member",
                NormalizedUserName = "MEMBER",
                Email = "member@vgs.com",
                NormalizedEmail = "MEMBER@VGS.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", // Password1!
                SecurityStamp = "HNGZXUROYPX527M6RHWO6OPYCETU2WVK",
                ConcurrencyStamp = "d91ba7e7-903d-453c-8caf-3ad1907f96c6",
                LockoutEnabled = true
            };

            builder.Entity<IdentityRole>().HasData(employeeRole, memberRole);

            builder.Entity<User>().HasData(employeeUser, memberUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = employeeUser.Id, RoleId = employeeRole.Id },
                new IdentityUserRole<string>() { UserId = memberUser.Id, RoleId = memberRole.Id });

            builder.Entity<Gender>().HasData(
                new Gender() { Id = 1, Name = "Male" },
                new Gender() { Id = 2, Name = "Female" },
                new Gender() { Id = 3, Name = "Other" });

            builder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "RPG" },
                new Category() { Id = 2, Name = "Racing" },
                new Category() { Id = 3, Name = "Sports" },
                new Category() { Id = 4, Name = "Simulation" },
                new Category() { Id = 5, Name = "FPS" },
                new Category() { Id = 6, Name = "Fighting" });

            builder.Entity<Platform>().HasData(
                new Platform() { Id = 1, Name = "PC" },
                new Platform() { Id = 2, Name = "Switch" },
                new Platform() { Id = 3, Name = "Xbox" },
                new Platform() { Id = 4, Name = "PlayStation" });
        }
    }
}