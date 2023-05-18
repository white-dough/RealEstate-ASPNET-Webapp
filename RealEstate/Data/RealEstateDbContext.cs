using RealEstate.Models;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Data
{

    public class RealEstateDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<PropertyModel> Property { get; set; }
        public DbSet<OrderModel> Order { get; set; }

        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options): base (options) { }
    }
}
