using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TeamChallenge.Models.Models;

namespace TeamChallenge.DbContext
{
    public class CosmeticStoreDbContext: IdentityDbContext<User>
    {
        public CosmeticStoreDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cosmetic>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);
        }        

        public DbSet<Category> Category { get; set; }
        public DbSet<DeliveryState> DeliveryState { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<Cosmetic> Cosmetiс { get; set; }
    }
}
