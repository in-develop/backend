using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.Models.Entities;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting up relationship one-to-many between CategoryEntity and SubCategoryEntity
            modelBuilder.Entity<SubCategoryEntity>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Setting up relationship
            modelBuilder.Entity<ProductSubCategoryEntity>()
                .HasKey(psc => new { psc.ProductId, psc.SubCategoryId });

            modelBuilder.Entity<ProductSubCategoryEntity>()
                .HasOne(psc => psc.Product)
                .WithMany(p => p.ProductSubCategories)
                .HasForeignKey(psc => psc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSubCategoryEntity>()
                .HasOne(psc => psc.SubCategory)
                .WithMany(sc => sc.ProductSubCategories)
                .HasForeignKey(psc => psc.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>()
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

            modelBuilder.Entity<ProductEntity>()
                .HasData(new ProductEntity
                {
                    Id = 1,
                    Name = "Prod 1",
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 1",
                },
                new ProductEntity
                {
                    Id = 2,
                    Name = "Prod 2",
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 2",
                },
                new ProductEntity
                {
                    Id = 3,
                    Name = "Prod 3",
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
        public DbSet<ReviewEntity> Reviews {  get; set; }
        public DbSet<SubCategoryEntity> SubCategories { get; set; }
    }
}
