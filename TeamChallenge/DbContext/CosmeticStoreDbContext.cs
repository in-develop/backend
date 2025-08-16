using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Entities;

namespace TeamChallenge.DbContext
{
    public class CosmeticStoreDbContext: IdentityDbContext<User>
    {
        public CosmeticStoreDbContext(DbContextOptions<CosmeticStoreDbContext> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
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
        public DbSet<CategoryCosmetic> CategoryCosmetic { get; set; }
    }
}
