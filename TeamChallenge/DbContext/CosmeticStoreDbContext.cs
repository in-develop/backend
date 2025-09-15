using Microsoft.AspNetCore.Identity;
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
<<<<<<< HEAD
            // One-to-many relationship between Category-Subcategories
            modelBuilder.Entity<CategoryEntity>()
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
=======
            base.OnModelCreating(builder);
            SetSeedData(builder);

        }   

        private void SetSeedData(ModelBuilder builder)
        {
            builder.Entity<UserEntity>()
                .HasData(new UserEntity()
                {
                    // username: admin; password : admin
                    Id = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEGX+x7oprDHdtrcw9g2r0B/J6Ae4IiS7/2HhEt4k6Zx7q3KtOmCXrvFrDxMlY8ox3A==",
                    EmailConfirmed = true,
                    SecurityStamp = "V4WTZVKR2NZW2BOK4YAEARQOCJHSV4SK",
                    ConcurrencyStamp = "cdca885a-43f5-4929-85c5-9b41dd697b37",
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SentEmailTime = DateTime.Parse("9/5/2025 11:43:34 PM")
                });

            builder.Entity<IdentityRole>()
                .HasData(new IdentityRole()
                {
                    Id = "d4a7c4fb-a129-47ff-b520-df1e8799d609",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "3f2f0e2e-2dcb-4f3c-8f7a-6e2e5f4c9b1a"
                });

            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = "d4a7c4fb-a129-47ff-b520-df1e8799d609",
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e"
                });
>>>>>>> master


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

<<<<<<< HEAD
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
=======
            builder.Entity<ProductBundleEntity>()
                .HasData(new ProductBundleEntity
                {
                    Id = 1,
                    Name = "Prod bundle 1",
                    StockQuantity = 10,
                    Price = 90.99m,
                    Description = "Description for product bundle 1",
                });

            builder.Entity<ProductEntity>()
>>>>>>> master
                .HasData(new ProductEntity
                {
                    Id = 1,
                    Name = "Prod 1",
                    StockQuantity = 100,
                    Price = 10.99m,
                    ProductBundleId = 1,
                    Description = "Description for product 1",
                    ProductSubCategories = [],
                },
                new ProductEntity
                {
                    Id = 2,
                    Name = "Prod 2",
                    StockQuantity = 100,
                    Price = 10.99m,
                    ProductBundleId = 1,
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

            builder.Entity<ReviewEntity>()
                .HasData(new ReviewEntity
                {
                    Id = 1,
                    ProductId = 1,
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
                    Rating = 5,
                    Comment = "Great product!"
                },
                new ReviewEntity
                {
                    Id = 2,
                    ProductId = 1,
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
                    Rating = 4,
                    Comment = "Good value for money."
                },
                new ReviewEntity
                {
                    Id = 3,
                    ProductId = 2,
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
                    Rating = 3,
                    Comment = "Average quality."
                });

            builder.Entity<CartEntity>()
                .HasData(new CartEntity
                {
                    Id = 1,
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e"
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
<<<<<<< HEAD
        public DbSet<SubCategoryEntity> SubCategories { get; set; }
        public DbSet<ProductSubCategoryEntity> productSubCategoryEntities { get; set; }
=======
        public DbSet<ProductBundleEntity> ProductBundles {  get; set; }
>>>>>>> master
    }
}
