USE [master]
GO
/****** Object:  Database [QLPhongTro]    Script Date: 12/28/2024 6:45:42 PM ******/
CREATE DATABASE [QLPhongTro]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLPhongTro_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\QLPhongTro.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QLPhongTro_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\QLPhongTro.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QLPhongTro] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLPhongTro].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLPhongTro] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLPhongTro] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLPhongTro] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLPhongTro] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLPhongTro] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLPhongTro] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLPhongTro] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLPhongTro] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLPhongTro] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLPhongTro] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLPhongTro] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLPhongTro] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLPhongTro] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLPhongTro] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLPhongTro] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLPhongTro] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLPhongTro] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLPhongTro] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLPhongTro] SET RECOVERY FULL 
GO
ALTER DATABASE [QLPhongTro] SET  MULTI_USER 
GO
ALTER DATABASE [QLPhongTro] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLPhongTro] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLPhongTro] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLPhongTro] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QLPhongTro] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QLPhongTro] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QLPhongTro', N'ON'
GO
ALTER DATABASE [QLPhongTro] SET QUERY_STORE = OFF
GO
USE [QLPhongTro]
GO
/****** Object:  Table [dbo].[ChiTietHoaDon]    Script Date: 12/28/2024 6:45:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHoaDon](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaHoaDon] [int] NULL,
	[MaPhong] [int] NULL,
	[TienPhong] [decimal](10, 3) NOT NULL,
	[TienDien] [decimal](10, 3) NOT NULL,
	[TienNuoc] [decimal](10, 3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dien]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dien](
	[MaDien] [int] IDENTITY(1,1) NOT NULL,
	[NgayDocSo] [date] NOT NULL,
	[ChiSoCu] [decimal](10, 3) NOT NULL,
	[ChiSoMoi] [decimal](10, 3) NULL,
	[GiaTien] [decimal](10, 3) NOT NULL,
	[MaPhong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDon]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDon](
	[MaHoaDon] [int] IDENTITY(1,1) NOT NULL,
	[NgayLap] [date] NOT NULL,
	[ThanhTien] [decimal](10, 3) NOT NULL,
	[MaPhong] [int] NULL,
	[MaKH] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHoaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachThue]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachThue](
	[MaKhachThue] [int] IDENTITY(1,1) NOT NULL,
	[HoTenDem] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](50) NOT NULL,
	[CMND] [nvarchar](100) NULL,
	[DienThoai] [nvarchar](20) NULL,
	[NgayVaoO] [date] NOT NULL,
	[NgayTraPhong] [date] NULL,
	[MaPhong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaKhachThue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nuoc]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nuoc](
	[MaNuoc] [int] IDENTITY(1,1) NOT NULL,
	[NgayDocSo] [date] NOT NULL,
	[ChiSoCu] [decimal](10, 3) NOT NULL,
	[ChiSoMoi] [decimal](10, 3) NULL,
	[GiaTien] [decimal](10, 3) NOT NULL,
	[MaPhong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNuoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phong]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phong](
	[MaPhong] [int] IDENTITY(1,1) NOT NULL,
	[SoPhong] [nvarchar](50) NOT NULL,
	[LoaiPhong] [nvarchar](50) NOT NULL,
	[GiaThueThang] [decimal](10, 3) NOT NULL,
	[DaThue] [bit] NOT NULL,
	[ImageUrl] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[MaTaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[SoDienThoai] [nvarchar](20) NOT NULL,
	[MatKhau] [nvarchar](100) NOT NULL,
	[ChucVu] [nvarchar](50) NOT NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YKienKhach]    Script Date: 12/28/2024 6:45:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YKienKhach](
	[MaYKien] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](50) NULL,
	[Email] [varchar](200) NULL,
	[NgayGui] [datetime] NULL,
	[NoiDung] [nvarchar](200) NULL,
	[TrangThai] [bit] NULL,
 CONSTRAINT [PK_YKienKhach] PRIMARY KEY CLUSTERED 
(
	[MaYKien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChiTietHoaDon] ON 

INSERT [dbo].[ChiTietHoaDon] ([MaChiTiet], [MaHoaDon], [MaPhong], [TienPhong], [TienDien], [TienNuoc]) VALUES (9, 10, 1, CAST(200000.000 AS Decimal(10, 3)), CAST(0.000 AS Decimal(10, 3)), CAST(200000.000 AS Decimal(10, 3)))
SET IDENTITY_INSERT [dbo].[ChiTietHoaDon] OFF
GO
SET IDENTITY_INSERT [dbo].[Dien] ON 

INSERT [dbo].[Dien] ([MaDien], [NgayDocSo], [ChiSoCu], [ChiSoMoi], [GiaTien], [MaPhong]) VALUES (2, CAST(N'2023-11-07' AS Date), CAST(22.000 AS Decimal(10, 3)), CAST(30.000 AS Decimal(10, 3)), CAST(20000.000 AS Decimal(10, 3)), 2)
INSERT [dbo].[Dien] ([MaDien], [NgayDocSo], [ChiSoCu], [ChiSoMoi], [GiaTien], [MaPhong]) VALUES (5, CAST(N'2023-11-07' AS Date), CAST(20.000 AS Decimal(10, 3)), NULL, CAST(20000.000 AS Decimal(10, 3)), 1)
SET IDENTITY_INSERT [dbo].[Dien] OFF
GO
SET IDENTITY_INSERT [dbo].[HoaDon] ON 

INSERT [dbo].[HoaDon] ([MaHoaDon], [NgayLap], [ThanhTien], [MaPhong], [MaKH]) VALUES (10, CAST(N'2023-12-05' AS Date), CAST(0.000 AS Decimal(10, 3)), 1, 9)
SET IDENTITY_INSERT [dbo].[HoaDon] OFF
GO
SET IDENTITY_INSERT [dbo].[KhachThue] ON 

INSERT [dbo].[KhachThue] ([MaKhachThue], [HoTenDem], [Ten], [CMND], [DienThoai], [NgayVaoO], [NgayTraPhong], [MaPhong]) VALUES (6, N'tran', N'dang', N'123456789', N'1234567890', CAST(N'2023-11-06' AS Date), NULL, 14)
INSERT [dbo].[KhachThue] ([MaKhachThue], [HoTenDem], [Ten], [CMND], [DienThoai], [NgayVaoO], [NgayTraPhong], [MaPhong]) VALUES (9, N'nam', N'nam', N'123456789', N'0377450981', CAST(N'2023-11-08' AS Date), CAST(N'2023-11-09' AS Date), 1)
SET IDENTITY_INSERT [dbo].[KhachThue] OFF
GO
SET IDENTITY_INSERT [dbo].[Nuoc] ON 

INSERT [dbo].[Nuoc] ([MaNuoc], [NgayDocSo], [ChiSoCu], [ChiSoMoi], [GiaTien], [MaPhong]) VALUES (3, CAST(N'2023-11-07' AS Date), CAST(20.000 AS Decimal(10, 3)), CAST(30.000 AS Decimal(10, 3)), CAST(20000.000 AS Decimal(10, 3)), 1)
SET IDENTITY_INSERT [dbo].[Nuoc] OFF
GO
SET IDENTITY_INSERT [dbo].[Phong] ON 

INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (1, N'110', N'Không gác', CAST(200000.000 AS Decimal(10, 3)), 1, N'a800a80e-a5ce-484e-b391-a4b45cba37d5.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (2, N'102', N'VIP', CAST(500000.000 AS Decimal(10, 3)), 0, N'adcf0aab-4600-4a68-9887-94e652f32cfe.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (3, N'103', N'Thường', CAST(200.000 AS Decimal(10, 3)), 0, N'f2239cab-8e1f-406c-aa34-2e12b6cba5b8.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (9, N'104', N'Thường', CAST(100.000 AS Decimal(10, 3)), 0, N'5840f2e0-be2e-4a8e-8e27-b9ac637d3594.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (11, N'105', N'Thường', CAST(150.000 AS Decimal(10, 3)), 0, N'89f93bbe-5f86-4f8a-a252-7e3a8a1d61fa.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (12, N'108', N'VIP', CAST(900.000 AS Decimal(10, 3)), 0, N'b0861a74-ff0d-422f-a9ea-b598b3e83dbf.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (13, N'109', N'VIP', CAST(800.000 AS Decimal(10, 3)), 0, N'00659e49-5249-4a0f-9ebb-063efeaa81ff.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (14, N'100', N'Thường', CAST(300.000 AS Decimal(10, 3)), 1, N'5e562db0-22a5-4509-a0db-d78929d07236.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (15, N'101', N'Thường', CAST(350.000 AS Decimal(10, 3)), 0, N'000d3db9-9d60-4701-931a-7d0ef0626610.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (16, N'106', N'Có gác', CAST(960000.000 AS Decimal(10, 3)), 0, N'9bbd4a87-0764-4b5d-9ad9-7207978ea27e.jpg')
INSERT [dbo].[Phong] ([MaPhong], [SoPhong], [LoaiPhong], [GiaThueThang], [DaThue], [ImageUrl]) VALUES (20, N'193', N'Có gác', CAST(510000.000 AS Decimal(10, 3)), 0, N'4e666d4e-a2f9-4a56-872b-5d7c47d1f44b.jpg')
SET IDENTITY_INSERT [dbo].[Phong] OFF
GO
SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 

INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [SoDienThoai], [MatKhau], [ChucVu], [TrangThai]) VALUES (1, N'0377450983', N'1234', N'Quản lý', 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [SoDienThoai], [MatKhau], [ChucVu], [TrangThai]) VALUES (3, N'0377450981', N'123', N'Khách hàng', 0)
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
GO
SET IDENTITY_INSERT [dbo].[YKienKhach] ON 

INSERT [dbo].[YKienKhach] ([MaYKien], [HoTen], [Email], [NgayGui], [NoiDung], [TrangThai]) VALUES (1, N'Thành', N'thanhrdark@gmail.com', CAST(N'2024-04-15T14:31:57.033' AS DateTime), N'OK', 1)
SET IDENTITY_INSERT [dbo].[YKienKhach] OFF
GO
ALTER TABLE [dbo].[ChiTietHoaDon]  WITH CHECK ADD FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HoaDon] ([MaHoaDon])
GO
ALTER TABLE [dbo].[ChiTietHoaDon]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[Dien]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD  CONSTRAINT [FK_HoaDon_KhachThue] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachThue] ([MaKhachThue])
GO
ALTER TABLE [dbo].[HoaDon] CHECK CONSTRAINT [FK_HoaDon_KhachThue]
GO
ALTER TABLE [dbo].[KhachThue]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[Nuoc]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
USE [master]
GO
ALTER DATABASE [QLPhongTro] SET  READ_WRITE 
GO
