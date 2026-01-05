--------------------------------------------------
-- CafeDB FULL (DROP TABLE + CREATE + SEED)
-- LOGIN HASH: MatKhauHash = MD5('hehe' + password) (HEX UPPER)
-- Login test (nếu appsettings DefaultAdmin là admin/admin):
--   NV002 / 123456
--   NV001 / 123456
--   admin / admin
--
-- DB-only FIX:
--   - NhanVien có TrangThai/FailedLoginCount/LockoutEndUtc (đúng lỗi bạn đang gặp)
--   - HoaDon có NhanVienId1 (đúng migrations trong zip)
--   - Tạo NhanVien Id=0 để không văng FK nếu client gửi NhanVienId=0
--------------------------------------------------

USE master;
IF DB_ID(N'CafeDB') IS NULL
    CREATE DATABASE CafeDB;
GO

USE CafeDB;
GO

SET NOCOUNT ON;
GO

--------------------------------------------------
-- 1) DROP TABLE (đúng thứ tự FK) + drop trigger nếu có
--------------------------------------------------
IF OBJECT_ID('dbo.TR_CTDG_RecalcTongTien','TR') IS NOT NULL
    DROP TRIGGER dbo.TR_CTDG_RecalcTongTien;
GO

IF OBJECT_ID('dbo.ThanhToan','U') IS NOT NULL DROP TABLE dbo.ThanhToan;
IF OBJECT_ID('dbo.ChiTietDonGoi','U') IS NOT NULL DROP TABLE dbo.ChiTietDonGoi;
IF OBJECT_ID('dbo.DonGoi','U') IS NOT NULL DROP TABLE dbo.DonGoi;
IF OBJECT_ID('dbo.HoaDon','U') IS NOT NULL DROP TABLE dbo.HoaDon;
IF OBJECT_ID('dbo.ThucUong','U') IS NOT NULL DROP TABLE dbo.ThucUong;
IF OBJECT_ID('dbo.ThucAn','U') IS NOT NULL DROP TABLE dbo.ThucAn;
IF OBJECT_ID('dbo.DanhMuc','U') IS NOT NULL DROP TABLE dbo.DanhMuc;
IF OBJECT_ID('dbo.Ban','U') IS NOT NULL DROP TABLE dbo.Ban;
IF OBJECT_ID('dbo.NhanVien','U') IS NOT NULL DROP TABLE dbo.NhanVien;
IF OBJECT_ID('dbo.AuditLog','U') IS NOT NULL DROP TABLE dbo.AuditLog;
GO

--------------------------------------------------
-- 2) CREATE TABLES
--------------------------------------------------

-- 2.1 NHÂN VIÊN (có thêm 3 cột đúng lỗi bạn đang gặp)
CREATE TABLE dbo.NhanVien
(
    Id               INT IDENTITY(1,1) PRIMARY KEY,
    MaNhanVien       NVARCHAR(50)  NOT NULL,
    TenDangNhap      NVARCHAR(50)  NOT NULL,
    HoTen            NVARCHAR(100) NOT NULL,
    VaiTro           NVARCHAR(50)  NOT NULL,
    MatKhauHash      NVARCHAR(200) NOT NULL,

    TrangThai        INT NOT NULL CONSTRAINT DF_NhanVien_TrangThai DEFAULT(1), -- 1=active,0=lock
    FailedLoginCount INT NOT NULL CONSTRAINT DF_NhanVien_Failed DEFAULT(0),
    LockoutEndUtc    DATETIME2(0) NULL
);
GO
CREATE UNIQUE INDEX UX_NhanVien_MaNhanVien  ON dbo.NhanVien(MaNhanVien);
CREATE UNIQUE INDEX UX_NhanVien_TenDangNhap ON dbo.NhanVien(TenDangNhap);
GO

-- 2.2 BÀN
CREATE TABLE dbo.Ban
(
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    TenBan    NVARCHAR(50) NOT NULL,
    TrangThai INT NOT NULL CONSTRAINT DF_Ban_TrangThai DEFAULT(0) -- 0=trống,1=đang dùng,2=đặt trước
);
GO
CREATE UNIQUE INDEX UX_Ban_TenBan ON dbo.Ban(TenBan);
GO

-- 2.3 DANH MỤC
CREATE TABLE dbo.DanhMuc
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Ten          NVARCHAR(100) NOT NULL,
    DangHoatDong BIT NOT NULL CONSTRAINT DF_DanhMuc_Active DEFAULT(1)
);
GO
CREATE UNIQUE INDEX UX_DanhMuc_Ten ON dbo.DanhMuc(Ten);
GO

-- 2.4 THỨC UỐNG
CREATE TABLE dbo.ThucUong
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    DanhMucId    INT NOT NULL,
    Ten          NVARCHAR(200) NOT NULL,
    DonGia       DECIMAL(18,2) NOT NULL,
    DangHoatDong BIT NOT NULL CONSTRAINT DF_ThucUong_Active DEFAULT(1),

    CONSTRAINT FK_ThucUong_DanhMuc
        FOREIGN KEY (DanhMucId) REFERENCES dbo.DanhMuc(Id)
);
GO

-- 2.5 THỨC ĂN
CREATE TABLE dbo.ThucAn
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    DanhMucId    INT NOT NULL,
    Ten          NVARCHAR(200) NOT NULL,
    DonGia       DECIMAL(18,2) NOT NULL,
    DangHoatDong BIT NOT NULL CONSTRAINT DF_ThucAn_Active DEFAULT(1),

    CONSTRAINT FK_ThucAn_DanhMuc
        FOREIGN KEY (DanhMucId) REFERENCES dbo.DanhMuc(Id)
);
GO

-- 2.6 ĐƠN GỌI
CREATE TABLE dbo.DonGoi
(
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    NhanVienId INT NOT NULL,
    BanId      INT NOT NULL,
    TrangThai  INT NOT NULL CONSTRAINT DF_DonGoi_Status DEFAULT(0), -- 0=mở,1=đã TT,2=hủy
    MoLuc      DATETIME2(0) NOT NULL,
    DongLuc    DATETIME2(0) NULL,
    GhiChu     NVARCHAR(255) NULL,

    CONSTRAINT FK_DonGoi_NhanVien FOREIGN KEY (NhanVienId) REFERENCES dbo.NhanVien(Id),
    CONSTRAINT FK_DonGoi_Ban      FOREIGN KEY (BanId)      REFERENCES dbo.Ban(Id),
    CONSTRAINT CK_DonGoi_TrangThai CHECK (TrangThai IN (0,1,2))
);
GO

-- 2.7 CHI TIẾT ĐƠN GỌI
CREATE TABLE dbo.ChiTietDonGoi
(
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    DonGoiId   INT NOT NULL,
    ThucUongId INT NULL,
    ThucAnId   INT NULL,
    SoLuong    INT NOT NULL,
    DonGia     DECIMAL(18,2) NOT NULL,
    ChietKhau  DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTDG_Discount DEFAULT(0),
    GhiChu     NVARCHAR(255) NULL,

    CONSTRAINT FK_CTDG_DonGoi   FOREIGN KEY (DonGoiId) REFERENCES dbo.DonGoi(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CTDG_ThucUong FOREIGN KEY (ThucUongId) REFERENCES dbo.ThucUong(Id),
    CONSTRAINT FK_CTDG_ThucAn   FOREIGN KEY (ThucAnId) REFERENCES dbo.ThucAn(Id),

    CONSTRAINT CK_CTDG_XOR CHECK (
        (ThucUongId IS NOT NULL AND ThucAnId IS NULL)
        OR
        (ThucUongId IS NULL AND ThucAnId IS NOT NULL)
    ),
    CONSTRAINT CK_CTDG_SoLuong CHECK (SoLuong > 0)
);
GO

-- 2.8 HÓA ĐƠN (có thêm NhanVienId1 đúng migrations)
CREATE TABLE dbo.HoaDon
(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon    NVARCHAR(50) NULL,
    NhanVienId  INT NOT NULL,
    NhanVienId1 INT NULL,            -- ✅ đúng migrations trong zip
    BanId       INT NULL,
    CreatedAt   DATETIME2(0) NOT NULL,
    ClosedAt    DATETIME2(0) NULL,
    TrangThai   TINYINT NOT NULL CONSTRAINT DF_HoaDon_Status DEFAULT(0),
    TongTien    DECIMAL(18,2) NOT NULL,
    GiamGia     DECIMAL(18,2) NOT NULL CONSTRAINT DF_HoaDon_Discount DEFAULT(0),
    Thue        DECIMAL(18,2) NOT NULL CONSTRAINT DF_HoaDon_Tax DEFAULT(0),
    GhiChu      NVARCHAR(255) NULL,

    CONSTRAINT FK_HoaDon_NhanVien  FOREIGN KEY (NhanVienId)  REFERENCES dbo.NhanVien(Id),
    CONSTRAINT FK_HoaDon_NhanVien1 FOREIGN KEY (NhanVienId1) REFERENCES dbo.NhanVien(Id),
    CONSTRAINT FK_HoaDon_Ban       FOREIGN KEY (BanId)       REFERENCES dbo.Ban(Id)
);
GO
CREATE UNIQUE INDEX UX_HoaDon_MaHoaDon_NotNull
ON dbo.HoaDon(MaHoaDon)
WHERE MaHoaDon IS NOT NULL;
GO

-- 2.9 THANH TOÁN
CREATE TABLE dbo.ThanhToan
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    DonGoiId     INT NOT NULL,
    SoTien       DECIMAL(18,2) NOT NULL,
    PhuongThuc   NVARCHAR(50) NOT NULL,
    ThanhToanLuc DATETIME2(0) NOT NULL,
    MaThamChieu  NVARCHAR(50) NULL,

    CONSTRAINT FK_ThanhToan_DonGoi FOREIGN KEY (DonGoiId) REFERENCES dbo.DonGoi(Id) ON DELETE CASCADE,
    CONSTRAINT CK_ThanhToan_SoTien CHECK (SoTien > 0)
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
    ThoiGian      DATETIME2(0) NOT NULL CONSTRAINT DF_AuditLog_Time DEFAULT SYSDATETIME()
);
GO

--------------------------------------------------
-- 3) SEED DATA
--   123456 => F9703EEBA357ECCC5FB7F98C039C366E   (MD5("hehe123456"))
--   admin  => 4574D080AE3048567ED09537D9A04793   (MD5("heheadmin"))
--------------------------------------------------

-- ✅ DB-only FIX: tạo NV Id=0
SET IDENTITY_INSERT dbo.NhanVien ON;
INSERT INTO dbo.NhanVien (Id, MaNhanVien, TenDangNhap, HoTen, VaiTro, MatKhauHash, TrangThai, FailedLoginCount, LockoutEndUtc)
VALUES (0, 'SYS0', 'SYS0', N'Hệ thống (Id=0)', 'System', 'F9703EEBA357ECCC5FB7F98C039C366E', 1, 0, NULL);
SET IDENTITY_INSERT dbo.NhanVien OFF;
GO

INSERT INTO dbo.NhanVien (MaNhanVien, TenDangNhap, HoTen, VaiTro, MatKhauHash)
VALUES
('AD001', 'admin', N'Quản trị hệ thống', 'Admin',    '4574D080AE3048567ED09537D9A04793'),
('NV001', 'NV001', N'Đỗ Minh Khoa',     'NhanVien', 'F9703EEBA357ECCC5FB7F98C039C366E'),
('NV002', 'NV002', N'Lê Gia Minh',      'NhanVien', 'F9703EEBA357ECCC5FB7F98C039C366E'),
('TG062', 'TG062', N'Lê Thị Hồng Thắm', N'Giữ xe',  'F9703EEBA357ECCC5FB7F98C039C366E');
GO

INSERT INTO dbo.Ban (TenBan, TrangThai)
VALUES
(N'Bàn 1',0),(N'Bàn 2',0),(N'Bàn 3',0),(N'Bàn 4',0),(N'Bàn 5',0),
(N'Bàn 6',0),(N'Bàn 7',0),(N'Bàn 8',0),(N'Bàn 9',0),(N'Bàn 10',0),
(N'Bàn 11',0),(N'Bàn 12',0),(N'Bàn 13',0),(N'Bàn 14',0),(N'Bàn 15',0);
GO

INSERT INTO dbo.DanhMuc (Ten, DangHoatDong)
VALUES
(N'Cà phê',1),
(N'Nước ép / Sinh tố',1),
(N'Đồ ăn vặt',1);
GO

INSERT INTO dbo.ThucAn (DanhMucId, Ten, DonGia, DangHoatDong)
SELECT Id, N'Bánh mì trứng', 25000, 1 FROM dbo.DanhMuc WHERE Ten=N'Đồ ăn vặt'
UNION ALL
SELECT Id, N'Khoai tây chiên', 30000, 1 FROM dbo.DanhMuc WHERE Ten=N'Đồ ăn vặt'
UNION ALL
SELECT Id, N'Bánh ngọt', 28000, 1 FROM dbo.DanhMuc WHERE Ten=N'Đồ ăn vặt';
GO

INSERT INTO dbo.ThucUong (DanhMucId, Ten, DonGia, DangHoatDong)
SELECT Id, N'Cà phê đen', 20000, 1 FROM dbo.DanhMuc WHERE Ten=N'Cà phê'
UNION ALL
SELECT Id, N'Cà phê sữa', 25000, 1 FROM dbo.DanhMuc WHERE Ten=N'Cà phê'
UNION ALL
SELECT Id, N'Bạc xỉu', 30000, 1 FROM dbo.DanhMuc WHERE Ten=N'Cà phê'
UNION ALL
SELECT Id, N'Nước cam', 30000, 1 FROM dbo.DanhMuc WHERE Ten=N'Nước ép / Sinh tố'
UNION ALL
SELECT Id, N'Sinh tố xoài', 35000, 1 FROM dbo.DanhMuc WHERE Ten=N'Nước ép / Sinh tố';
GO

--------------------------------------------------
-- 4) Seed DonGoi / ChiTiet / HoaDon / ThanhToan (KHÔNG assume Id 1..3)
--------------------------------------------------
DECLARE
    @NV001 INT = (SELECT TOP 1 Id FROM dbo.NhanVien WHERE MaNhanVien='NV001'),
    @NV002 INT = (SELECT TOP 1 Id FROM dbo.NhanVien WHERE MaNhanVien='NV002'),
    @Ban1  INT = (SELECT TOP 1 Id FROM dbo.Ban WHERE TenBan=N'Bàn 1'),
    @Ban2  INT = (SELECT TOP 1 Id FROM dbo.Ban WHERE TenBan=N'Bàn 2'),
    @Ban3  INT = (SELECT TOP 1 Id FROM dbo.Ban WHERE TenBan=N'Bàn 3'),
    @Ban13 INT = (SELECT TOP 1 Id FROM dbo.Ban WHERE TenBan=N'Bàn 13');

DECLARE @DG1 INT, @DG2 INT, @DG3 INT, @DG13 INT;

-- DonGoi bàn 1
INSERT INTO dbo.DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES (@NV001, @Ban1, 1, DATEADD(MINUTE,-30,SYSDATETIME()), SYSDATETIME(), N'Đơn gọi mẫu cho bàn 1');
SET @DG1 = SCOPE_IDENTITY();

-- DonGoi bàn 2
INSERT INTO dbo.DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES (@NV001, @Ban2, 1, DATEADD(DAY,-1,SYSDATETIME()), DATEADD(HOUR,1,DATEADD(DAY,-1,SYSDATETIME())), N'Đơn gọi mẫu cho bàn 2');
SET @DG2 = SCOPE_IDENTITY();

-- DonGoi bàn 3
INSERT INTO dbo.DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES (@NV002, @Ban3, 1, DATEADD(DAY,-2,SYSDATETIME()), DATEADD(HOUR,2,DATEADD(DAY,-2,SYSDATETIME())), N'Đơn gọi mẫu cho bàn 3');
SET @DG3 = SCOPE_IDENTITY();

-- Lấy id thức uống theo tên (không assume)
DECLARE
    @TU_Den   INT = (SELECT TOP 1 Id FROM dbo.ThucUong WHERE Ten=N'Cà phê đen'),
    @TU_Sua   INT = (SELECT TOP 1 Id FROM dbo.ThucUong WHERE Ten=N'Cà phê sữa'),
    @TU_BacXiu INT = (SELECT TOP 1 Id FROM dbo.ThucUong WHERE Ten=N'Bạc xỉu'),
    @TU_Cam   INT = (SELECT TOP 1 Id FROM dbo.ThucUong WHERE Ten=N'Nước cam'),
    @TU_Xoai  INT = (SELECT TOP 1 Id FROM dbo.ThucUong WHERE Ten=N'Sinh tố xoài');

-- ChiTiet
INSERT INTO dbo.ChiTietDonGoi (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
(@DG1, @TU_Den,   NULL, 2, 20000, 0, N'2 ly cà phê đen'),
(@DG1, @TU_Sua,   NULL, 1, 25000, 0, N'1 ly cà phê sữa'),
(@DG2, @TU_Sua,   NULL, 1, 25000, 0, N'1 ly cà phê sữa'),
(@DG2, @TU_BacXiu,NULL, 1, 30000, 0, N'1 ly bạc xỉu'),
(@DG3, @TU_Cam,   NULL, 1, 30000, 0, N'1 ly nước cam'),
(@DG3, @TU_Xoai,  NULL, 1, 35000, 0, N'1 ly sinh tố xoài');

-- HoaDon (set luôn NhanVienId1 = NhanVienId để navigation HoaDons không bị rỗng)
INSERT INTO dbo.HoaDon (MaHoaDon, NhanVienId, NhanVienId1, BanId, CreatedAt, ClosedAt, TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
('HD0001', @NV001, @NV001, @Ban1,  DATEADD(MINUTE,-30,SYSDATETIME()), SYSDATETIME(), 1, 65000, 0, 0, N'Hóa đơn mẫu bàn 1'),
('HD0002', @NV001, @NV001, @Ban2,  DATEADD(DAY,-1,SYSDATETIME()),     DATEADD(HOUR,1,DATEADD(DAY,-1,SYSDATETIME())), 1, 55000, 0, 0, N'Hóa đơn mẫu bàn 2'),
('HD0003', @NV002, @NV002, @Ban3,  DATEADD(DAY,-2,SYSDATETIME()),     DATEADD(HOUR,2,DATEADD(DAY,-2,SYSDATETIME())), 1, 65000, 0, 0, N'Hóa đơn mẫu bàn 3');

-- ThanhToan
INSERT INTO dbo.ThanhToan (DonGoiId, SoTien, PhuongThuc, ThanhToanLuc, MaThamChieu)
VALUES
(@DG1, 65000, N'Tiền mặt',     SYSDATETIME(),                 'TT0001'),
(@DG2, 55000, N'Chuyển khoản', DATEADD(DAY,-1,SYSDATETIME()), 'TT0002'),
(@DG3, 65000, N'Tiền mặt',     DATEADD(DAY,-2,SYSDATETIME()), 'TT0003');

-- Don đang mở bàn 13
INSERT INTO dbo.DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES (@NV001, @Ban13, 0, DATEADD(MINUTE,-15,SYSDATETIME()), NULL, N'Đơn đang phục vụ bàn 13');
SET @DG13 = SCOPE_IDENTITY();

INSERT INTO dbo.ChiTietDonGoi (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
(@DG13, @TU_Den, NULL, 2, 20000, 0, N'2 ly cà phê đen'),
(@DG13, @TU_Sua, NULL, 1, 25000, 0, N'1 ly cà phê sữa');

UPDATE dbo.Ban SET TrangThai = 1 WHERE Id = @Ban13;

PRINT N'✅ DONE. Login: NV002/123456, NV001/123456, admin/admin';
PRINT N'✅ DB-only fix: đã tạo NhanVien Id=0 và đủ cột TrangThai/FailedLoginCount/LockoutEndUtc + HoaDon.NhanVienId1';
GO
