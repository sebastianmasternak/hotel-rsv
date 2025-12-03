using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class HotelDbContext : DbContext
    { 
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed a RoomTypes 
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { Id = 1, Name = "Basic", Description = "Basic Room with bed and bathroom", PricePerNight = 50.00m },
                new RoomType { Id = 2, Name = "Double", Description = "Double bed for two guests and bathroom.", PricePerNight = 80.00m },
                new RoomType { Id = 3, Name = "Deluxe", Description = "Larger room with Tv and wifi.", PricePerNight = 130.00m },
                new RoomType { Id = 4, Name = "Suite", Description = "Separate living area and bedroom.", PricePerNight = 220.00m },
                new RoomType { Id = 5, Name = "Family", Description = "Spacious room for families (up to 4).", PricePerNight = 160.00m }
            );

        }
    }
}
