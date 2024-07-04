using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Models;

public partial class ItshopMaew123Context : DbContext
{
    public ItshopMaew123Context()
    {
    }

    public ItshopMaew123Context(DbContextOptions<ItshopMaew123Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressSaleSnapshot> AddressSaleSnapshots { get; set; }

    public virtual DbSet<Flagnoti> Flagnotis { get; set; }

    public virtual DbSet<GenerateNumber> GenerateNumbers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCatagory> ProductCatagories { get; set; }

    public virtual DbSet<ProductStock> ProductStocks { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    public virtual DbSet<WishListItem> WishListItems { get; set; }

    public virtual DbSet<OtpEntity> OtpEntities { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }
    public virtual DbSet<Amphoe> Amphoes { get; set; }
    public virtual DbSet<Tambol> Tambols { get; set; }

    public virtual DbSet<DecreasedProduct> DecreasedProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Thai_100_CI_AS");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Address");

            entity.ToTable("Address");

            entity.Property(e => e.addressName).HasColumnName("addressName");
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Address_User");
        });

        modelBuilder.Entity<AddressSaleSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AddressSaleSnapshot");

            entity.ToTable("AddressSaleSnapshot");

            entity.Property(e => e.AddressName).HasColumnName("addressName");
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.AddressSaleSnapshots)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AddressSaleSnapshot_User");
        });

        modelBuilder.Entity<Flagnoti>(entity =>
        {
            entity.HasKey(e => e.FlagnotiId).HasName("PK_Flagnoti_1");

            entity.ToTable("Flagnoti");

            entity.Property(e => e.FlagnotiName)
                .HasMaxLength(3)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
        });

        modelBuilder.Entity<GenerateNumber>(entity =>
        {
            entity.HasKey(e => new { e.Year, e.Month });

            entity.ToTable("GenerateNumber");

        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Product_1");

            entity.ToTable("Product");

            entity.Property(e => e.Condition)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ImgPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.ProductCatagory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCatagoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCatagory");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductType");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Products)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_Product_Promotion");
        });

        modelBuilder.Entity<ProductCatagory>(entity =>
        {
            entity.HasKey(e => e.ProductCatagoryId).HasName("PK_ProductCatagory_1");

            entity.ToTable("ProductCatagory");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductCatagoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasKey(e => e.ProductStockId).HasName("PK_ProductStock_1");

            entity.ToTable("ProductStock");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductStock_Product");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK_ProductType_1");

            entity.ToTable("ProductType");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.ProductTypes)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductType_ProductCatagory");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK_Promotion_1");

            entity.ToTable("Promotion");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PromotionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PromotionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK_Sale_1");

            entity.ToTable("Sale");

            entity.Property(e => e.SaleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SaleTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ImgPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Annotation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ParcelTypeNo)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ParcelNumber)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Status).WithMany(p => p.Sales)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Status");

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_User");

            entity.HasOne(d => d.AddressSnapshot).WithMany(p => p.Sales)
                .HasForeignKey(d => d.AddressSnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_AddressSaleSnapshot");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(e => new { e.SaleId, e.ProductId });

            entity.ToTable("SaleItem");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Product");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Sale");

            entity.HasOne(d => d.Promotion).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Promotion");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK_Status_1");

            entity.ToTable("Status");

            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK_Token_1");

            entity.ToTable("Token");

            entity.Property(e => e.Tokenkey)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");

            entity.HasOne(d => d.User).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Token_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User_1");

            entity.ToTable("User");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.InsertBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
            entity.Property(e => e.Salt)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserTel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("Thai_CI_AS");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListId).HasName("PK_WishList_1");

            entity.ToTable("WishList");

            entity.HasOne(d => d.User).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_WishList_User");
        });

        modelBuilder.Entity<WishListItem>(entity =>
        {
            entity.HasKey(e => e.WishListItemId).HasName("PK_WishList_Item_1");

            entity.ToTable("WishListItem");

            entity.HasOne(d => d.Product).WithMany(p => p.WishListItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_WishListItem_Product");

            entity.HasOne(d => d.WishList).WithMany(p => p.WishListItems)
                .HasForeignKey(d => d.WishListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WishListItem_WishList");
        });

        modelBuilder.Entity<OtpEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OtpEntity");

            entity.ToTable("OtpEntity");

            entity.Property(e => e.OtpCode)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.Jti)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Otps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OtpEntity_User");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Pcode);

            entity.ToTable("province");

            entity.Property(e => e.Pcode)
                .ValueGeneratedNever()
                .HasColumnName("pcode");
            entity.Property(e => e.Pname).HasColumnName("pname");
            entity.Property(e => e.TypeSoilder).HasColumnName("type_soilder");
        });

        modelBuilder.Entity<Amphoe>(entity =>
        {
            entity.HasKey(e => e.Acode);

            entity.ToTable("amphoe");

            entity.Property(e => e.Acode)
                .ValueGeneratedNever()
                .HasColumnName("acode");
            entity.Property(e => e.Aname).HasColumnName("aname");
            entity.Property(e => e.Pcode).HasColumnName("pcode");
            entity.Property(e => e.Pname).HasColumnName("pname");

            entity.HasOne(d => d.PcodeNavigation).WithMany(p => p.Amphoes)
                .HasForeignKey(d => d.Pcode)
                .HasConstraintName("FK_amphoe_province");
        });

        modelBuilder.Entity<Tambol>(entity =>
        {
            entity.HasKey(e => e.Tcode);

            entity.ToTable("tambol");

            entity.Property(e => e.Tcode)
                .ValueGeneratedNever()
                .HasColumnName("tcode");
            entity.Property(e => e.Acode).HasColumnName("acode");
            entity.Property(e => e.Aname).HasColumnName("aname");
            entity.Property(e => e.Pcode).HasColumnName("pcode");
            entity.Property(e => e.Pname).HasColumnName("pname");
            entity.Property(e => e.Tname).HasColumnName("tname");

            entity.HasOne(d => d.AcodeNavigation).WithMany(p => p.Tambols)
                .HasForeignKey(d => d.Acode)
                .HasConstraintName("FK_tambol_amphoe");

            entity.HasOne(d => d.PcodeNavigation).WithMany(p => p.Tambols)
                .HasForeignKey(d => d.Pcode)
                .HasConstraintName("FK_tambol_province");
        });

        modelBuilder.Entity<DecreasedProduct>(entity =>
        {
            entity.HasKey(e => e.DecreasedProductId);
                entity.ToTable("DecreasedProduct");

            entity.Property(e => e.DecreaseBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DecreaseReason)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_DecreasedProduct_Product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
