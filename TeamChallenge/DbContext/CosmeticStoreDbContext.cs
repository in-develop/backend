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

        protected override void OnModelCreating(ModelBuilder builder)
        {
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

            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = "d4a7c4fb-a129-47ff-b520-df1e8799d609",
                    UserId = "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e"
                });

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
                    Name = "Prod 1",
                    CategoryId = 1,
                    StockQuantity = 100,
                    Price = 10.99m,
                    ProductBundleId = 1,
                    Description = "Description for product 1",
                },
                new ProductEntity
                {
                    Id = 2,
                    Name = "Prod 2",
                    CategoryId = 2,
                    StockQuantity = 100,
                    Price = 10.99m,
                    ProductBundleId = 1,
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
    }
}
