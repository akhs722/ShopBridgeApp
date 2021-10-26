using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoBridge.DataAccessLayer.Models
{
    public partial class ShopBridgeDBContext : DbContext
    {
        public ShopBridgeDBContext()
        {
        }

        public ShopBridgeDBContext(DbContextOptions<ShopBridgeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<Products> Products { get; set; }
       

        [DbFunction("ufn_GenerateNewProductId", "dbo")]
        public static string GenerateNewProductId()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source =localhost\\SQLEXPRESS;Initial Catalog=ShopBridgeDB;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("pk_CategoryId");

                entity.HasIndex(e => e.CategoryName)
                    .HasName("uq_CategoryName")
                    .IsUnique();

                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductImages>(entity =>
            {
                entity.HasKey(e => e.Sno)
                    .HasName("pk_Sno");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_ProductId");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("pk_ProductId");

                entity.HasIndex(e => e.ProductName)
                    .HasName("uq_ProductName")
                    .IsUnique();

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Price).HasColumnType("numeric(8, 0)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_CategoryId");
            });

           
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
