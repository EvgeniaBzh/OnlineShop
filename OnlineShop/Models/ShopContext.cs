using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shop.Models;

public partial class ShopContext : DbContext
{
    public ShopContext()
    {
    }

    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Representative> Representatives { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DHK8L7H\\SQLEXPRESS;Database=OnlineShop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(e => e.ApartmentNumber).HasMaxLength(10);
            entity.Property(e => e.HouseNumber).HasMaxLength(10);
            entity.Property(e => e.Settlement).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.LastEdited).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.LastEdited).HasDefaultValueSql("GETDATE()");

        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.LastEdited).HasDefaultValueSql("GETDATE()");

        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Status);

        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.Password).HasMaxLength(50);

            entity.HasOne(d => d.Notification)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK_Customers_Notifications");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DiscountCode).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Dimensions).HasColumnType("float");

        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Feedbacks) 
                .HasForeignKey(d => d.CustomerId) 
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedbacks_Customers");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.Feedbacks) 
                .HasForeignKey(d => d.ProductId) 
                .OnDelete(DeleteBehavior.Cascade) 
                .HasConstraintName("FK_Feedbacks_Products");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.NotificationType)
                .WithMany(p => p.Notifications) 
                .HasForeignKey(d => d.TypeId) 
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_NotificationTypes");
        });

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");
            entity.HasIndex(e => e.ShippingAddressId, "IX_Orders_ShippingAddressId");

            entity.Property(e => e.LastEditted).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShippingDate).HasColumnType("datetime");

            entity.HasOne(d => d.Courier)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CourierId)
                .HasConstraintName("FK_Orders_Couriers");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.Payment)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Orders_Payments");

            entity.HasOne(d => d.ShippingAddress)
                .WithMany(p => p.Orders) 
                .HasForeignKey(d => d.ShippingAddressId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.OrderProducts)
                .WithOne(p => p.Order)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Orders_Products");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.HasIndex(e => e.ProductId, "IX_OrderProducts_ProductId");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Генерація Id
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.Weight).HasPrecision(10, 2).IsRequired();
            entity.Property(e => e.StockQuantity).IsRequired();
            entity.Property(e => e.Image).HasMaxLength(200).IsRequired();
            entity.Property(e => e.LastEdited).HasDefaultValueSql("GETDATE()");

            entity.HasMany(d => d.Feedbacks)
                .WithOne(p => p.Product)
                .HasForeignKey(d => d.ProductId) // Зовнішній ключ у відгуку
                .OnDelete(DeleteBehavior.Cascade) // Видалення продукту видаляє відгуки
                .HasConstraintName("FK_Feedbacks_Products");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Products_Brands");

            entity.HasOne(d => d.Representative)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.RepresentativeId)
                .HasConstraintName("FK_Products_Representatives");

            entity.HasMany(d => d.OrderProducts)
                .WithOne(p => p.Product)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Products_OrderProducts");
        });

        modelBuilder.Entity<Representative>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Password).HasMaxLength(50).IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasMany(d => d.Products)
                .WithOne(p => p.Representative)
                .HasForeignKey(d => d.RepresentativeId)
                .HasConstraintName("FK_Products_Representatives");

            entity.HasMany(d => d.Stores)
                .WithOne(p => p.Representative)
                .HasForeignKey(d => d.RepresentativeId)
                .HasConstraintName("FK_Stores_Representatives");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Representative)
                .WithMany(p => p.Stores)
                .HasForeignKey(d => d.RepresentativeId)
                .HasConstraintName("FK_Stores_Representatives");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
