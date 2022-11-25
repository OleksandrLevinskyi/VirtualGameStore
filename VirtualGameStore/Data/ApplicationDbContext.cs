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
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

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

            builder.Entity<Game>()
                .HasMany(g => g.Categories)
                .WithMany(c => c.Games)
                .UsingEntity<GameCategory>();

            builder.Entity<Game>()
                .HasMany(g => g.Platforms)
                .WithMany(c => c.Games)
                .UsingEntity<GamePlatform>();

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

            builder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

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

            var memberUser1 = new User()
            {
                Id = "d5dafa9f-92a4-43dc-9652-02cf3860d621",
                UserName = "msmith",
                FirstName = "Mark",
                LastName = "Smith",
                NormalizedUserName = "MSMITH",
                Email = "msmith@vgs.com",
                NormalizedEmail = "MSMITH@VGS.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", // Password1!
                SecurityStamp = "WQ7TGOMDYEUVSMNVX2G35VKZ4MPGODG4",
                ConcurrencyStamp = "0f4fa02d-33c6-48e6-b573-7218fa00c9a2",
                LockoutEnabled = true
            };

            var memberUser2 = new User()
            {
                Id = "76742c46-0008-4749-af77-5d129b6d88b1",
                UserName = "bjacobs",
                FirstName = "Bob",
                LastName = "Jacobs",
                NormalizedUserName = "BJACOBS",
                Email = "bjacobs@vgs.com",
                NormalizedEmail = "BJACOBS@VGS.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", // Password1!
                SecurityStamp = "7XDDKAH2YGWTBDC7UVPT76DUXTLQES3E",
                ConcurrencyStamp = "0c6c0c5a-52ea-4b4c-89cc-8130611f1e54",
                LockoutEnabled = true
            };

            var memberUser3 = new User()
            {
                Id = "76742c46-0008-4749-af77-5d129b6d88b1",
                UserName = "rjohnson",
                FirstName = "Rosa",
                LastName = "Johnson",
                NormalizedUserName = "RJOHNSON",
                Email = "rjohnson@vgs.com",
                NormalizedEmail = "RJOHNSON@VGS.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGiel0OKEa5+pKsFTlka1xHjptYHOzHiRtImi2E8QYR4dgXVvcAFZm1AA7wKbxO9ew==", // Password1!
                SecurityStamp = "7XDDKAH2YGWTBDC7UVPT76DUXTLQES3E",
                ConcurrencyStamp = "0c6c0c5a-52ea-4b4c-89cc-8130611f1e54",
                LockoutEnabled = true
            };

            builder.Entity<IdentityRole>().HasData(employeeRole, memberRole);

            builder.Entity<User>().HasData(employeeUser, memberUser, memberUser1, memberUser2, memberUser3);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = employeeUser.Id, RoleId = employeeRole.Id },
                new IdentityUserRole<string>() { UserId = memberUser.Id, RoleId = memberRole.Id },
                new IdentityUserRole<string>() { UserId = memberUser1.Id, RoleId = memberRole.Id },
                new IdentityUserRole<string>() { UserId = memberUser2.Id, RoleId = memberRole.Id });

            builder.Entity<Gender>().HasData(
                new Gender() { Id = 1, Name = "Male" },
                new Gender() { Id = 2, Name = "Female" },
                new Gender() { Id = 3, Name = "Other" });

            var categories = new List<Category>() {
                new Category() { Id = 1, Name = "RPG" },
                new Category() { Id = 2, Name = "Racing" },
                new Category() { Id = 3, Name = "Sports" },
                new Category() { Id = 4, Name = "Simulation" },
                new Category() { Id = 5, Name = "FPS" },
                new Category() { Id = 6, Name = "Fighting" }
            };

            builder.Entity<Category>().HasData(categories);

            var platforms = new List<Platform>()
            {
                new Platform() { Id = 1, Name = "PC" },
                new Platform() { Id = 2, Name = "Switch" },
                new Platform() { Id = 3, Name = "Xbox" },
                new Platform() { Id = 4, Name = "PlayStation" }
            };

            builder.Entity<Platform>().HasData(platforms);

            var games = new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Minecraft Dungeons",
                    Description = "Fight your way through an exciting action-adventure game, inspired by classic dungeon crawlers and set in the Minecraft universe!",
                    IsDigital = false,
                    Price = 25.99,
                    Stock = 15
                },
                new Game()
                {
                    Id = 2,
                    Name = "Among Us",
                    Description = "An online and local party game of teamwork and betrayal for 4-15 players...in space!",
                    IsDigital = false,
                    Price = 5.69,
                    Stock = 5
                },
                new Game()
                {
                    Id = 3,
                    Name = "Sonic Frontiers",
                    Description = "Experience Sonic like never before!",
                    IsDigital = true,
                    Price = 79.99,
                    Stock = 0
                },
                new Game()
                {
                    Id = 4,
                    Name = "Lego Worlds",
                    Description = "Experience a galaxy of Worlds made entirely from LEGO bricks. EXPLORE gigantic landscapes, DISCOVER countless surprises, and CREATE anything you can imagine by building with LEGO bricks.",
                    IsDigital = true,
                    Price = 29.99,
                    Stock = 0
                }
            };

            builder.Entity<Game>().HasData(games);

            builder.Entity<GameCategory>().HasData(
                new GameCategory() { GameId = games[0].Id, CategoryId = categories[0].Id },
                new GameCategory() { GameId = games[0].Id, CategoryId = categories[1].Id },
                new GameCategory() { GameId = games[1].Id, CategoryId = categories[3].Id },
                new GameCategory() { GameId = games[2].Id, CategoryId = categories[3].Id },
                new GameCategory() { GameId = games[2].Id, CategoryId = categories[4].Id },
                new GameCategory() { GameId = games[2].Id, CategoryId = categories[5].Id },
                new GameCategory() { GameId = games[3].Id, CategoryId = categories[2].Id },
                new GameCategory() { GameId = games[3].Id, CategoryId = categories[3].Id },
                new GameCategory() { GameId = games[3].Id, CategoryId = categories[5].Id });

            builder.Entity<GamePlatform>().HasData(
                new GamePlatform() { GameId = games[0].Id, PlatformId = platforms[0].Id },
                new GamePlatform() { GameId = games[0].Id, PlatformId = platforms[3].Id },
                new GamePlatform() { GameId = games[1].Id, PlatformId = platforms[2].Id },
                new GamePlatform() { GameId = games[2].Id, PlatformId = platforms[2].Id },
                new GamePlatform() { GameId = games[2].Id, PlatformId = platforms[3].Id },
                new GamePlatform() { GameId = games[3].Id, PlatformId = platforms[1].Id },
                new GamePlatform() { GameId = games[3].Id, PlatformId = platforms[3].Id });

            var events = new List<Event>()
            {
                new Event(){
                    Id=1,
                    Name="Winter Hackathon 2022",
                    Description="The Growth Hacking Hackathon is an event where designers, developers and marketers come together to create tools to help accelerate growth.",
                    AttendeeLimit=5,
                    CreatorId=employeeUser.Id,
                    DateTime=DateTime.Now.AddDays(5)
                },
                new Event(){
                    Id=2,
                    Name="Holiday Trivia",
                    Description="Come join us for some virtual pub trivia! Show up solo or with a team — and join the league to be eligible for big prizes!",
                    AttendeeLimit=16,
                    CreatorId=employeeUser.Id,
                    DateTime=DateTime.Now.AddDays(75).AddHours(14)
                },
                new Event(){
                    Id=3,
                    Name="Dungeons & Dragons",
                    Description="We will be playing an adventure provided in this fantastic series of separate stories set in the Forgotten Realms library itself, known as Candlekeep.",
                    AttendeeLimit=1,
                    CreatorId=employeeUser.Id,
                    DateTime=DateTime.Now.AddDays(1).AddHours(5)
                },
            };

            builder.Entity<Event>().HasData(events);
        }
    }
}