using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLPhong.Models;

public partial class QlphongTroContext : DbContext
{
    public QlphongTroContext()
    {
    }

    public QlphongTroContext(DbContextOptions<QlphongTroContext> QLPhongTro)
        : base(QLPhongTro)
    {
    }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<Dien> Diens { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachThue> KhachThues { get; set; }

    public virtual DbSet<Nuoc> Nuocs { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
    public virtual DbSet<YKienKhach> YKienKhachs { get; set; }
 


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(" Data Source=DESKTOP-4Q0P439\\MSSQLSERVER02;Initial Catalog=QLPhongTro;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__ChiTietH__CDF0A1149F156D0C");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.TienDien).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.TienNuoc).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.TienPhong).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__ChiTietHo__MaHoa__4316F928");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__ChiTietHo__MaPho__440B1D61");
        });

        modelBuilder.Entity<Dien>(entity =>
        {
            entity.HasKey(e => e.MaDien).HasName("PK__Dien__33326024903EA424");

            entity.ToTable("Dien");

            entity.Property(e => e.ChiSoCu).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ChiSoMoi).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.GiaTien).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Diens)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__Dien__MaPhong__44FF419A");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13B662A27AA");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_HoaDon_KhachThue");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__HoaDon__MaPhong__45F365D3");
        });

        modelBuilder.Entity<KhachThue>(entity =>
        {
            entity.HasKey(e => e.MaKhachThue).HasName("PK__KhachThu__1C7173F781A9E928");

            entity.ToTable("KhachThue");

            entity.Property(e => e.Cmnd)
                .HasMaxLength(100)
                .HasColumnName("CMND");
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.HoTenDem).HasMaxLength(50);
            entity.Property(e => e.Ten).HasMaxLength(50);

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.KhachThues)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__KhachThue__MaPho__47DBAE45");
        });

        modelBuilder.Entity<Nuoc>(entity =>
        {
            entity.HasKey(e => e.MaNuoc).HasName("PK__Nuoc__21306FEA9F7191DE");

            entity.ToTable("Nuoc");

            entity.Property(e => e.ChiSoCu).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ChiSoMoi).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.GiaTien).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Nuocs)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__Nuoc__MaPhong__48CFD27E");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__Phong__20BD5E5BB68415D8");

            entity.ToTable("Phong");

            entity.Property(e => e.GiaThueThang).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.LoaiPhong).HasMaxLength(50);
            entity.Property(e => e.SoPhong).HasMaxLength(50);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C652905FBC4E4");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
        });
        modelBuilder.Entity<YKienKhach>(entity =>
        {
            entity.ToTable("YKienKhach");

            entity.HasKey(e => e.MaYKien);

            entity.Property(e => e.MaYKien)
                .HasColumnName("MaYKien")
                .IsRequired()
                .ValueGeneratedOnAdd(); // Thiết lập cột MaYKien để tự động tăng

            entity.Property(e => e.HoTen).HasColumnName("HoTen").HasMaxLength(200);
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(200);
            entity.Property(e => e.NgayGui).HasColumnName("NgayGui").HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasColumnName("NoiDung").HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasColumnName("TrangThai").IsRequired();
        });

         


            OnModelCreatingPartial(modelBuilder);
        }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
