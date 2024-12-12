using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPINatureHub3.Models;

public partial class NatureHub3Context : DbContext
{
    public NatureHub3Context()
    {
    }

    public NatureHub3Context(DbContextOptions<NatureHub3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Bookmark> Bookmarks { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<HealthTip> HealthTips { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Remedy> Remedies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-5RJDN9Q;database=NatureHub3;trusted_connection=true;trustservercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1B50DC9ECA");

            entity.ToTable("Address");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Address__85FB4E388E4ABC12").IsUnique();

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ZipCode).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Address__UserID__5DCAEF64");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E8780A10C1");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Username, "UQ__Admin__536C85E4B4A2A6D1").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admin__A9D105349CEF6636").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Admins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Admin_Role");
        });

        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => e.BookmarkId).HasName("PK__Bookmark__541A3A9187495ACD");

            entity.ToTable("Bookmark");

            entity.Property(e => e.BookmarkId).HasColumnName("BookmarkID");
            entity.Property(e => e.RemedyId).HasColumnName("RemedyID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Remedy).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.RemedyId)
                .HasConstraintName("FK__Bookmark__Remedy__619B8048");

            entity.HasOne(d => d.User).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookmark__UserID__60A75C0F");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797A6ABF7CF");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cart__ProductID__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__UserID__5070F446");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BA5012CD5");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E094269398").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(20);
        });

        modelBuilder.Entity<HealthTip>(entity =>
        {
            entity.HasKey(e => e.TipId).HasName("PK__HealthTi__2DB1A1A80A6F57EF");

            entity.Property(e => e.TipId).HasColumnName("TipID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedByAdminId).HasColumnName("CreatedByAdminID");
            entity.Property(e => e.TipTitle).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.HealthTips)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__HealthTip__Categ__5812160E");

            entity.HasOne(d => d.CreatedByAdmin).WithMany(p => p.HealthTips)
                .HasForeignKey(d => d.CreatedByAdminId)
                .HasConstraintName("FK__HealthTip__Creat__59063A47");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDEDC61A33");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedByAdminId).HasColumnName("CreatedByAdminID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(30);
            entity.Property(e => e.StockQuantity).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__4BAC3F29");

            entity.HasOne(d => d.CreatedByAdmin).WithMany(p => p.Products)
                .HasForeignKey(d => d.CreatedByAdminId)
                .HasConstraintName("FK__Products__Create__4CA06362");
        });

        modelBuilder.Entity<Remedy>(entity =>
        {
            entity.HasKey(e => e.RemedyId).HasName("PK__Remedies__4A8F36AB3BC3F46E");

            entity.Property(e => e.RemedyId).HasColumnName("RemedyID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedByAdminId).HasColumnName("CreatedByAdminID");
            entity.Property(e => e.RemedyName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Remedies)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Remedies__Catego__5441852A");

            entity.HasOne(d => d.CreatedByAdmin).WithMany(p => p.Remedies)
                .HasForeignKey(d => d.CreatedByAdminId)
                .HasConstraintName("FK__Remedies__Create__5535A963");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A2492EBDD");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61602EA20F7D").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(10);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACFCEB8264");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A4CD5D10").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
