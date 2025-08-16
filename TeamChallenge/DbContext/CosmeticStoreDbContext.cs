using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Entities;

namespace TeamChallenge.DbContext
{
    public class CosmeticStoreDbContext: IdentityDbContext<UserEntity>
    {
        public CosmeticStoreDbContext(DbContextOptions<CosmeticStoreDbContext> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CategoryEntity>()
                .HasData(new CategoryEntity
                {
                    Id = 1,
                    Name = "Category 1"
                },
                new CategoryEntity
                {
                    Id = 2,
                    Name = "Category 2"
                });

            builder.Entity<ProductEntity>()
                .HasData(new ProductEntity
                {
                    Id = 1,
                    Name = "Prod 1",
                    CategoryId = 1,
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 1",
                },
                new ProductEntity
                {
                    Id = 2,
                    Name = "Prod 2",
                    CategoryId = 2,
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 2",
                },
                new ProductEntity
                {
                    Id = 3,
                    Name = "Prod 3",
                    CategoryId = 1,
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 3",
                });
        }   

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartItemEntity> Cartitems { get; set; }
        public DbSet<UserEntity> Users {  get; set; }
    }
}
