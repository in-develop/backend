using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Static_data;

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

            // Setting up relationship Many-to-Many between Products-SubCategories

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductSubCategoryEntity>()
                .HasOne(psc => psc.Product)
                .WithMany(p => p.ProductSubCategories)
                .HasForeignKey(psc => psc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSubCategoryEntity>()
                .HasOne(psc => psc.SubCategory)
                .WithMany(sc => sc.ProductSubCategories)
                .HasForeignKey(psc => psc.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            SetSeedData(modelBuilder);

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
                    Name = GlobalConstants.Roles.Admin,
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "3f2f0e2e-2dcb-4f3c-8f7a-6e2e5f4c9b1a"
                });

            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = "d4a7c4fb-a129-47ff-b520-df1e8799d609",
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e"
                });

            builder.Entity<CategoryEntity>().HasData(
                new CategoryEntity { Id = 1, Name = "Face Care" },
                new CategoryEntity { Id = 2, Name = "Makeup" },
                new CategoryEntity { Id = 3, Name = "Hair Care" }
            );

            builder.Entity<SubCategoryEntity>()
                .HasData(new SubCategoryEntity { Id = 1, Name = "Cleansers", CategoryId = 1 },
                    new SubCategoryEntity { Id = 2, Name = "Moisturizers", CategoryId = 1 },
                    new SubCategoryEntity { Id = 3, Name = "Foundation", CategoryId = 2 },
                    new SubCategoryEntity { Id = 4, Name = "Lipstick", CategoryId = 2 },
                    new SubCategoryEntity { Id = 5, Name = "Shampoo", CategoryId = 3 }
                );

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
                .HasData(new ProductEntity
                    {
                        Id = 1,
                        Name = "Gentle Hydrating Cleanser",
                        Description = "A gentle, non-stripping cleanser for all skin types.",
                        Price = 15.99m,
                        StockQuantity = 100,
                        
                    },
                    new ProductEntity
                    {
                        Id = 2,
                        Name = "Daily Defense Moisturizer SPF 30",
                        Description = "A lightweight moisturizer with broad-spectrum sun protection.",
                        Price = 28.50m,
                        StockQuantity = 80,
                    },
                    new ProductEntity
                    {
                        Id = 3,
                        Name = "Luminous Silk Foundation",
                        Description = "A buildable, medium-coverage foundation with a radiant finish.",
                        Price = 45.00m,
                        StockQuantity = 60,
                    },
                    new ProductEntity
                    {
                        Id = 4,
                        Name = "Velvet Matte Lipstick - Classic Red",
                        Description = "A long-lasting, highly pigmented matte lipstick.",
                        Price = 22.00m,
                        StockQuantity = 120,
                    },
                    new ProductEntity
                    {
                        Id = 5,
                        Name = "Volume Boost Shampoo",
                        Description = "Adds body and shine to fine, flat hair.",
                        Price = 18.00m,
                        StockQuantity = 90,
                    });

            builder.Entity<ProductSubCategoryEntity>().HasData(
                // Link "Gentle Hydrating Cleanser" (ProductId=1) to "Cleansers" (SubCategoryId=1)
                new { Id = 1, ProductId = 1, SubCategoryId = 1 },

                // Link "Daily Defense Moisturizer" (ProductId=2) to "Moisturizers" (SubCategoryId=2)
                new { Id = 2, ProductId = 2, SubCategoryId = 2 },

                // Link "Luminous Silk Foundation" (ProductId=3) to "Foundation" (SubCategoryId=3)
                new { Id = 3, ProductId = 3, SubCategoryId = 3 },

                // Link "Velvet Matte Lipstick" (ProductId=4) to "Lipstick" (SubCategoryId=4)
                new { Id = 4, ProductId = 4, SubCategoryId = 4 },

                // Link "Volume Boost Shampoo" (ProductId=5) to "Shampoo" (SubCategoryId=5)
                new { Id = 5, ProductId = 5, SubCategoryId = 5 },

                // --- Example of a product in multiple subcategories ---
                // Let's say the Foundation can also be considered a Moisturizer
                new { Id = 6, ProductId = 3, SubCategoryId = 2 }
            );

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
        public DbSet<ProductBundleEntity> ProductBundles {  get; set; }
        public DbSet<SubCategoryEntity> SubCategories { get; set; }
        public DbSet<ProductSubCategoryEntity> ProductSubCategories { get; set; }
        public DbSet<OrderHistoryEntity> OrderHistory {  get; set; }
    }
}
