using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using SpyStore.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using SpyStore.Models.ViewModels;

namespace SpyStore.Dal.EfStructures
{
    public class StoreContext : DbContext
    {
        // Map SQL Server Functions to C#
        [DbFunction("GetOrderTotal", Schema = "Store")]
        public static int GetOrderTotal(int orderId)
        {
            //code in here doesn't matter
            throw new Exception();
        }
        public int CustomerId { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }

        // Views
        // Obsolete
        //public DbQuery<CartRecordWithProductInfo> CartRecordWithProductInfos { get; set; }
        public DbSet<CartRecordWithProductInfo> CartRecordWithProductInfos { get; set; }
        public DbSet<OrderDetailWithProductInfo> OrderDetailWithProductInfos { get; set; }


        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).HasName("IX_Customers").IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e =>
                     e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e =>
                     e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");

                entity.Property(e => e.OrderTotal).HasColumnType("money")
                      .HasComputedColumnSql("Store.GetOrderTotal([Id])");
            });

            // Instead of adding the customer id to every query, a global query filter is used to add a
            // where clause to every query limiting the results to the customer id set on the context.
            // This filter can be ignored in LINQ using the IgnoreQueryFilters()
            modelBuilder.Entity<Order>().HasQueryFilter(x => x.CustomerId == CustomerId);

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.LineItemTotal).HasColumnType("money")
                        .HasComputedColumnSql("[Quantity]*[UnitCost]");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.CurrentPrice).HasColumnType("money");

                // The Fluent API is used to override the default naming conventions for Owned entities.
                entity.OwnsOne(o => o.Details,
                pd =>
                {
                    pd.Property(p =>
                       p.Description).HasColumnName(nameof(ProductDetails.Description));
                    pd.Property(p =>
                       p.ModelName).HasColumnName(nameof(ProductDetails.ModelName));
                    pd.Property(p =>
                       p.ModelNumber).HasColumnName(nameof(ProductDetails.ModelNumber));
                    pd.Property(p =>
                       p.ProductImage).HasColumnName(nameof(ProductDetails.ProductImage));
                    pd.Property(p =>
                       p.ProductImageLarge).HasColumnName(nameof(ProductDetails.ProductImageLarge));
                    pd.Property(p =>
                       p.ProductImageThumb).HasColumnName(nameof(ProductDetails.ProductImageThumb));
                });

            });

            modelBuilder.Entity<ShoppingCartRecord>(entity =>
            {
                entity.Property(e => e.DateCreated)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Quantity).HasDefaultValue(1);

                entity.HasIndex(e => new { ShoppingCartRecordId = e.Id, e.ProductId, e.CustomerId })
                      .HasName("IX_ShoppingCart").IsUnique();

            });

            modelBuilder.Entity<ShoppingCartRecord>().HasQueryFilter(x => x.CustomerId == CustomerId);

            //To map the view model to the view in the OnModelCreating method:
            //modelBuilder.Query<CartRecordWithProductInfo>().ToView("CartRecordWithProductInfo", "Store");
            modelBuilder.Entity<CartRecordWithProductInfo>().ToView("CartRecordWithProductInfo", "Store");

            modelBuilder.Entity<OrderDetailWithProductInfo>().ToView("OrderDetailWithProductInfo", "Store");

        }
    }
}
