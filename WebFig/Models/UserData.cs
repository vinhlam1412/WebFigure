using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebFig.Models
{
    public partial class UserData : DbContext
    {
        public UserData()
            : base("name=UserData")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Gioithieu> Gioithieux { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<NSX> NSXes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Coupoun> Coupouns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.SoDT)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Cart>()
                .Property(e => e.gia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Category>()
                .Property(e => e.hinhCategory)
                .IsUnicode(false);

            modelBuilder.Entity<NSX>()
                .Property(e => e.hinhNSX)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.gia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.hinh)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.hinhDetail1)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.hinhDetail2)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.hinhDetail3)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Coupoun>()
                .Property(e => e.codename)
                .IsFixedLength();

            modelBuilder.Entity<Coupoun>()
                .Property(e => e.quantity)
                .IsFixedLength();

            modelBuilder.Entity<Coupoun>()
                .Property(e => e.price)
                .HasPrecision(18, 0);
        }
    }
}
