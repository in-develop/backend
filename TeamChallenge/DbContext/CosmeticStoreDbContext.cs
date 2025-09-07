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
            // One-to-many relationship between Category-Subcategories
            modelBuilder.Entity<CategoryEntity>()
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            // Setting up relationship Many-to-Many between Products-SubCategories
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
                    Name = "For Face",
                },
                new CategoryEntity
                {
                    Id = 2,
                    Name = "Category 2",
                });

            modelBuilder.Entity<SubCategoryEntity>()
                .HasData(new SubCategoryEntity
                {
                    Id = 3,
                    Name = "Facial Mask",
                    CategoryId = 1,
                },
                new SubCategoryEntity {
                    Id = 4,
                    Name = "Facial Spray",
                    CategoryId = 1,
                });

            modelBuilder.Entity<ProductEntity>()
                .HasData(new ProductEntity
                {
                    Id = 1,
                    Name = "Prod 1",
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 1",
                    ProductSubCategories = [],
                },
                new ProductEntity
                {
                    Id = 2,
                    Name = "Prod 2",
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 2",
                    ProductSubCategories = [],
                },
                new ProductEntity
                {
                    Id = 3,
                    Name = "Prod 3",
                    StockQuantity = 100,
                    Price = 10.99m,
                    Description = "Description for product 3",
                    ProductSubCategories = [],
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
        public DbSet<ProductSubCategoryEntity> productSubCategoryEntities { get; set; }
    }
}
