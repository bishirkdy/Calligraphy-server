using System;
using System.Collections.Generic;
using System.Text;
using Calligraphy.Domain.Entities; 
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        public DbSet<WishlistItem> WishlistItems { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Inquiry> Inquiries { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //carrt -> user   one to one 
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);//delete user -> delt cart

            // one user 1 cart
            modelBuilder.Entity<Cart>()
                 .HasIndex(c => c.UserId)
                 .IsUnique();

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)// one to many
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            //cartitem -> product
            modelBuilder.Entity<CartItem>()
                  .HasOne(ci => ci.Product)
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
               .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // wishlist user
            modelBuilder.Entity<Wishlist>()
               .HasOne(w => w.User)
               .WithMany()
               .HasForeignKey(w => w.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // wishlistitem -> product
            modelBuilder.Entity<WishlistItem>()
              .HasOne(wi => wi.Product)
              .WithMany()
              .HasForeignKey(wi => wi.ProductId)
             .OnDelete(DeleteBehavior.Restrict);


            //wishlstitem ->  wishlist
            modelBuilder.Entity<WishlistItem>()
               .HasOne(wi => wi.Wishlist)
               .WithMany(w => w.WishlistItems)
               .HasForeignKey(wi => wi.WishlistId)
               .OnDelete(DeleteBehavior.Cascade);


            // same prdtct cant added twice
            modelBuilder.Entity<WishlistItem>()
            .HasIndex(wi => new { wi.WishlistId, wi.ProductId })
            .IsUnique();

            modelBuilder.Entity<Category>()
               .HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            // oreder->orderitem

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                 .HasOne(o => o.User)
                 .WithMany()
                 .HasForeignKey(o => o.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            //orderitem -> product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                 .WithMany()
                 .HasForeignKey(oi => oi.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);

            modelBuilder.Entity<Payment>()
                 .Property(p => p.Amount)
                 .HasPrecision(18, 2);

            modelBuilder.Entity<Category>().HasData(
                 new Category
                 {
                      Id = 1,
                     Name = "SACRED QURANIC VERSES"
                 },
                 new Category
                 {
                    Id = 2,
                    Name = "MODERN ALBHABET ARTS"
                 },
                 new Category
                 {
                       Id = 3,
                      Name = "BESPOKE COMMISIONS"
                 }
             );

            modelBuilder.Entity<Address>()
                 .HasOne(a => a.User)
                 .WithMany()
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

