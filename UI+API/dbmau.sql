--------------------------------------------------
-- 0. TẠO DATABASE (NẾU CHƯA CÓ) + CHỌN DB
--------------------------------------------------
IF DB_ID('CafeDB') IS NULL
BEGIN
    CREATE DATABASE CafeDB;
END
GO

USE CafeDB;
GO

--------------------------------------------------
-- 1. XÓA BẢNG CŨ THEO THỨ TỰ RÀNG BUỘC FK
--------------------------------------------------
IF OBJECT_ID('dbo.ChiTietDonGoi', 'U') IS NOT NULL DROP TABLE dbo.ChiTietDonGoi;
IF OBJECT_ID('dbo.ThanhToan'   , 'U') IS NOT NULL DROP TABLE dbo.ThanhToan;
IF OBJECT_ID('dbo.HoaDon'      , 'U') IS NOT NULL DROP TABLE dbo.HoaDon;
IF OBJECT_ID('dbo.DonGoi'      , 'U') IS NOT NULL DROP TABLE dbo.DonGoi;
IF OBJECT_ID('dbo.ThucUong'    , 'U') IS NOT NULL DROP TABLE dbo.ThucUong;
IF OBJECT_ID('dbo.ThucAn'      , 'U') IS NOT NULL DROP TABLE dbo.ThucAn;
IF OBJECT_ID('dbo.DanhMuc'     , 'U') IS NOT NULL DROP TABLE dbo.DanhMuc;
IF OBJECT_ID('dbo.Ban'         , 'U') IS NOT NULL DROP TABLE dbo.Ban;
IF OBJECT_ID('dbo.NhanVien'    , 'U') IS NOT NULL DROP TABLE dbo.NhanVien;
IF OBJECT_ID('dbo.AuditLog'    , 'U') IS NOT NULL DROP TABLE dbo.AuditLog;
GO

--------------------------------------------------
-- 2. TẠO BẢNG
--------------------------------------------------

-- 2.1 NHÂN VIÊN
--  TenDangNhap dùng cho login & seeder
CREATE TABLE dbo.NhanVien
(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaNhanVien  NVARCHAR(50)  NOT NULL UNIQUE,
    TenDangNhap NVARCHAR(50)  NOT NULL UNIQUE,
    HoTen       NVARCHAR(100) NOT NULL,
    VaiTro      NVARCHAR(50)  NOT NULL,       -- Admin / NhanVien / ...
    MatKhauHash NVARCHAR(200) NOT NULL
);
GO

-- 2.2 BÀN
-- TrangThai: 0 = Trống, 1 = Đang dùng, 2 = Đặt trước
CREATE TABLE dbo.Ban
(
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    TenBan    NVARCHAR(50) NOT NULL,
    TrangThai INT NOT NULL DEFAULT(0)
);
GO

-- 2.3 DANH MỤC
CREATE TABLE dbo.DanhMuc
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Ten          NVARCHAR(100) NOT NULL,
    DangHoatDong BIT NOT NULL DEFAULT(1)
);
GO

-- 2.4 THỨC UỐNG
CREATE TABLE dbo.ThucUong
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    DanhMucId    INT NOT NULL,
    Ten          NVARCHAR(100) NOT NULL,
    DonGia       DECIMAL(18,2) NOT NULL,
    DangHoatDong BIT NOT NULL DEFAULT(1),

    CONSTRAINT FK_ThucUong_DanhMuc
        FOREIGN KEY (DanhMucId) REFERENCES dbo.DanhMuc(Id)
);
GO

-- 2.5 THỨC ĂN
CREATE TABLE dbo.ThucAn
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    DanhMucId    INT NOT NULL,
    Ten          NVARCHAR(100) NOT NULL,
    DonGia       DECIMAL(18,2) NOT NULL,
    DangHoatDong BIT NOT NULL DEFAULT(1),

    CONSTRAINT FK_ThucAn_DanhMuc
        FOREIGN KEY (DanhMucId) REFERENCES dbo.DanhMuc(Id)
);
GO

-- 2.6 ĐƠN GỌI
-- TrangThai: 0 = đang mở, 1 = đã thanh toán, 2 = hủy
CREATE TABLE dbo.DonGoi
(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    NhanVienId  INT NOT NULL,
    BanId       INT NOT NULL,
    TrangThai   INT NOT NULL DEFAULT(0),
    MoLuc       DATETIME NOT NULL,
    DongLuc     DATETIME NULL,
    GhiChu      NVARCHAR(255) NULL,

    CONSTRAINT FK_DonGoi_NhanVien FOREIGN KEY (NhanVienId) REFERENCES dbo.NhanVien(Id),
    CONSTRAINT FK_DonGoi_Ban      FOREIGN KEY (BanId)      REFERENCES dbo.Ban(Id)
);
GO

-- 2.7 CHI TIẾT ĐƠN GỌI
CREATE TABLE dbo.ChiTietDonGoi
(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    DonGoiId    INT NOT NULL,
    ThucUongId  INT NULL,
    ThucAnId    INT NULL,
    SoLuong     INT NOT NULL,
    DonGia      DECIMAL(18,2) NOT NULL,
    ChietKhau   DECIMAL(18,2) NOT NULL DEFAULT(0),
    GhiChu      NVARCHAR(255) NULL,

    CONSTRAINT FK_ChiTietDonGoi_DonGoi   FOREIGN KEY (DonGoiId)   REFERENCES dbo.DonGoi(Id),
    CONSTRAINT FK_ChiTietDonGoi_ThucUong FOREIGN KEY (ThucUongId) REFERENCES dbo.ThucUong(Id),
    CONSTRAINT FK_ChiTietDonGoi_ThucAn   FOREIGN KEY (ThucAnId)   REFERENCES dbo.ThucAn(Id)
);
GO

-- 2.8 HÓA ĐƠN
-- TrangThai: 0 = mở, 1 = đã thanh toán
CREATE TABLE dbo.HoaDon
(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon    NVARCHAR(50) NOT NULL UNIQUE,
    NhanVienId  INT NOT NULL,
    BanId       INT NOT NULL,
    CreatedAt   DATETIME NOT NULL,
    ClosedAt    DATETIME NULL,
    TrangThai   INT NOT NULL DEFAULT(0),
    TongTien    DECIMAL(18,2) NOT NULL,
    GiamGia     DECIMAL(18,2) NOT NULL DEFAULT(0),
    Thue        DECIMAL(18,2) NOT NULL DEFAULT(0),
    GhiChu      NVARCHAR(255) NULL,

    CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY (NhanVienId) REFERENCES dbo.NhanVien(Id),
    CONSTRAINT FK_HoaDon_Ban      FOREIGN KEY (BanId)      REFERENCES dbo.Ban(Id)
);
GO

-- 2.9 THANH TOÁN
CREATE TABLE dbo.ThanhToan
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    DonGoiId     INT NOT NULL,
    SoTien       DECIMAL(18,2) NOT NULL,
    PhuongThuc   NVARCHAR(50) NOT NULL,
    ThanhToanLuc DATETIME NOT NULL,
    MaThamChieu  NVARCHAR(50) NULL,

    CONSTRAINT FK_ThanhToan_DonGoi FOREIGN KEY (DonGoiId) REFERENCES dbo.DonGoi(Id)
);
GO

-- 2.10 AUDIT LOG
CREATE TABLE dbo.AuditLog
(
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    TenBang       NVARCHAR(MAX) NOT NULL,
    IdBanGhi      INT NULL,
    HanhDong      NVARCHAR(MAX) NOT NULL,
    GiaTriCu      NVARCHAR(MAX) NULL,
    GiaTriMoi     NVARCHAR(MAX) NULL,
    NguoiThucHien NVARCHAR(MAX) NULL,
    ThoiGian      DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

--------------------------------------------------
-- 3. SEED DỮ LIỆU MẪU (CHA → CON)
--------------------------------------------------

-- 3.1 NHÂN VIÊN
INSERT INTO dbo.NhanVien (MaNhanVien, TenDangNhap, HoTen, VaiTro, MatKhauHash)
VALUES
    ('AD001', 'admin1', N'Quản trị hệ thống', 'Admin',    '123456'),
    ('NV001', 'NV001',  N'Đỗ Minh Khoa',      'NhanVien', '123456'),
    ('NV002', 'NV002',  N'Lê Gia Minh',       'NhanVien', '123456'),
    ('TG062', 'TG062',  N'Lê Thị Hồng Thắm',  N'Giữ xe',  '123456');
GO

-- 3.2 BÀN (15 bàn, mặc định trống)
INSERT INTO dbo.Ban (TenBan, TrangThai)
VALUES
    (N'Bàn 1',  0),
    (N'Bàn 2',  0),
    (N'Bàn 3',  0),
    (N'Bàn 4',  0),
    (N'Bàn 5',  0),
    (N'Bàn 6',  0),
    (N'Bàn 7',  0),
    (N'Bàn 8',  0),
    (N'Bàn 9',  0),
    (N'Bàn 10', 0),
    (N'Bàn 11', 0),
    (N'Bàn 12', 0),
    (N'Bàn 13', 0),
    (N'Bàn 14', 0),
    (N'Bàn 15', 0);
GO

-- 3.3 DANH MỤC
INSERT INTO dbo.DanhMuc (Ten, DangHoatDong)
VALUES
    (N'Cà phê',             1),
    (N'Nước ép / Sinh tố',  1),
    (N'Đồ ăn vặt',          1);
GO
-- Id: 1 = Cà phê, 2 = Nước ép / Sinh tố, 3 = Đồ ăn vặt

-- 3.4 THỨC ĂN (DanhMucId = 3)
INSERT INTO dbo.ThucAn (DanhMucId, Ten, DonGia, DangHoatDong)
VALUES
    (3, N'Bánh mì trứng',    25000, 1),
    (3, N'Khoai tây chiên',  30000, 1),
    (3, N'Bánh ngọt',        28000, 1);
GO

-- 3.5 THỨC UỐNG (DanhMucId = 1,2)
INSERT INTO dbo.ThucUong (DanhMucId, Ten, DonGia, DangHoatDong)
VALUES
    (1, N'Cà phê đen',    20000.00, 1),
    (1, N'Cà phê sữa',    25000.00, 1),
    (1, N'Bạc xỉu',       30000.00, 1),
    (2, N'Nước cam',      30000.00, 1),
    (2, N'Sinh tố xoài',  35000.00, 1);
GO

-- 3.6 ĐƠN GỌI MẪU (ĐÃ THANH TOÁN) BÀN 1,2,3
INSERT INTO dbo.DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES
    (2, 1, 1,
     DATEADD(MINUTE, -30, GETDATE()),
     GETDATE(),
     N'Đơn gọi mẫu cho bàn 1'),
    (2, 2, 1,
     DATEADD(DAY, -1, GETDATE()),
     DATEADD(DAY, -1, DATEADD(HOUR, 1, GETDATE())),
     N'Đơn gọi mẫu cho bàn 2'),
    (3, 3, 1,
     DATEADD(DAY, -2, GETDATE()),
     DATEADD(DAY, -2, DATEADD(HOUR, 2, GETDATE())),
     N'Đơn gọi mẫu cho bàn 3');
GO
-- DonGoi Id = 1,2,3

-- 3.7 CHI TIẾT ĐƠN GỌI CHO 3 ĐƠN TRÊN
INSERT INTO dbo.ChiTietDonGoi
    (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
    (1, 1, NULL, 2, 20000.00, 0.00, N'2 ly cà phê đen'),
    (1, 2, NULL, 1, 25000.00, 0.00, N'1 ly cà phê sữa'),

    (2, 2, NULL, 1, 25000.00, 0.00, N'1 ly cà phê sữa'),
    (2, 3, NULL, 1, 30000.00, 0.00, N'1 ly bạc xỉu'),

    (3, 4, NULL, 1, 30000.00, 0.00, N'1 ly nước cam'),
    (3, 5, NULL, 1, 35000.00, 0.00, N'1 ly sinh tố xoài');
GO

-- 3.8 HÓA ĐƠN CHO 3 ĐƠN TRÊN
INSERT INTO dbo.HoaDon
    (MaHoaDon, NhanVienId, BanId,
     CreatedAt,                ClosedAt,
     TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
    ('HD0001', 2, 1,
     DATEADD(MINUTE, -30, GETDATE()), GETDATE(),
     1, 65000.00, 0.00, 0.00, N'Hóa đơn mẫu cho bàn 1');

INSERT INTO dbo.HoaDon
    (MaHoaDon, NhanVienId, BanId,
     CreatedAt,                       ClosedAt,
     TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
    ('HD0002', 2, 2,
     DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, DATEADD(HOUR, 1, GETDATE())),
     1, 55000.00, 0.00, 0.00, N'Hóa đơn mẫu cho bàn 2');

INSERT INTO dbo.HoaDon
    (MaHoaDon, NhanVienId, BanId,
     CreatedAt,                       ClosedAt,
     TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
    ('HD0003', 3, 3,
     DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -2, DATEADD(HOUR, 2, GETDATE())),
     1, 65000.00, 0.00, 0.00, N'Hóa đơn mẫu cho bàn 3');
GO

-- 3.9 THANH TOÁN CHO 3 ĐƠN TRÊN
INSERT INTO dbo.ThanhToan
    (DonGoiId, SoTien, PhuongThuc, ThanhToanLuc, MaThamChieu)
VALUES
    (1, 65000.00, N'Tiền mặt',     GETDATE(),                       'TT0001'),
    (2, 55000.00, N'Chuyển khoản', DATEADD(DAY, -1, GETDATE()),     'TT0002'),
    (3, 65000.00, N'Tiền mặt',     DATEADD(DAY, -2, GETDATE()),     'TT0003');
GO

--------------------------------------------------
-- 3.10 ĐƠN ĐANG MỞ (TEST FORM BÁN HÀNG)
--       BÀN 13: đang phục vụ (TrangThai DonGoi = 0)
--------------------------------------------------
DECLARE @DonGoiBan13 INT;

INSERT INTO dbo.DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES
(
    2,              -- NV001
    13,             -- Bàn 13
    0,              -- 0 = đang mở, chưa thanh toán
    DATEADD(MINUTE, -15, GETDATE()),
    NULL,
    N'Đơn đang phục vụ bàn 13'
);

SET @DonGoiBan13 = SCOPE_IDENTITY();

INSERT INTO dbo.ChiTietDonGoi
    (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
    (@DonGoiBan13, 1, NULL, 2, 20000.00, 0.00, N'2 ly cà phê đen'),
    (@DonGoiBan13, 2, NULL, 1, 25000.00, 0.00, N'1 ly cà phê sữa');

-- Cập nhật trạng thái bàn 13 = ĐANG DÙNG
UPDATE dbo.Ban
SET TrangThai = 1
WHERE Id = 13;
GO

--------------------------------------------------
-- 4. KIỂM TRA NHANH
--------------------------------------------------
SELECT * FROM dbo.NhanVien;
SELECT * FROM dbo.Ban;
SELECT * FROM dbo.DanhMuc;
SELECT * FROM dbo.ThucUong;
SELECT * FROM dbo.ThucAn;
SELECT * FROM dbo.DonGoi;
SELECT * FROM dbo.ChiTietDonGoi;
SELECT * FROM dbo.HoaDon;
SELECT * FROM dbo.ThanhToan;
GO
