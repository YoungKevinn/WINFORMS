INSERT INTO NhanVien (MaNhanVien, HoTen, VaiTro, MatKhauHash)
VALUES
    ('admin1', N'Quản trị hệ thống', 'Admin',  '123456'),
    ('NV001', N'Đỗ Minh Khoa',      'NhanVien', '123456'),
    ('NV002', N'Lê Gia Minh',       'NhanVien', '123456');
    ('TG062', N'Lê Thị Hồng Thắm',       'Giữ XE', '123456');
    INSERT INTO Ban (TenBan, TrangThai)
VALUES
    (N'Bàn 1', 1),
    (N'Bàn 2',1),
    (N'Bàn 3', 1),
    (N'Bàn 4', 1),
    (N'Bàn 5',1);
    INSERT INTO DanhMuc (Ten,DangHoatDong)
VALUES
    (N'Cà phê',1),
    (N'Nước ép / Sinh tố',1),
    (N'Đồ ăn vặt',1);
    INSERT INTO ThucAn (DanhMucId, Ten, DonGia,DangHoatDong)
VALUES
    (3, N'Bánh mì trứng',    25000,1),
    (3, N'Khoai tây chiên',  30000,1),
    (3, N'Bánh ngọt',        28000,1);
    INSERT INTO ThucUong (DanhMucId, Ten, DonGia, DangHoatDong)
VALUES
    (1, N'Cà phê đen',    20000.00, 1),  -- Id = 1
    (1, N'Cà phê sữa',    25000.00, 1),  -- Id = 2
    (1, N'Bạc xỉu',       30000.00, 1),  -- Id = 3
    (2, N'Nước cam',      30000.00, 1),  -- Id = 4
    (2, N'Sinh tố xoài',  35000.00, 1);  -- Id = 5
GO

-- Đơn gọi 1: hôm nay, NV001 (Id=2), Bàn 1
INSERT INTO DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES
(
    2,                          -- NhanVienId (NV001)
    1,                          -- BanId  (Bàn 1)
    1,                          -- TrangThai: 1 = đã thanh toán
    DATEADD(MINUTE, -30, GETDATE()),  -- MoLuc: mở lúc 30' trước
    GETDATE(),                  -- DongLuc: đóng lúc bây giờ
    N'Đơn gọi mẫu cho bàn 1'
);

-- Đơn gọi 2: hôm qua, NV001 (Id=2), Bàn 2
INSERT INTO DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES
(
    2,
    2,
    1,
    DATEADD(DAY, -1, GETDATE()),        -- MoLuc: hôm qua
    DATEADD(DAY, -1, DATEADD(HOUR, 1, GETDATE())), -- DongLuc: hôm qua + 1h
    N'Đơn gọi mẫu cho bàn 2'
);

-- Đơn gọi 3: 2 ngày trước, NV002 (Id=3), Bàn 3
INSERT INTO DonGoi (NhanVienId, BanId, TrangThai, MoLuc, DongLuc, GhiChu)
VALUES
(
    3,
    3,
    1,
    DATEADD(DAY, -2, GETDATE()),        -- MoLuc: 2 ngày trước
    DATEADD(DAY, -2, DATEADD(HOUR, 2, GETDATE())), -- DongLuc: 2 ngày trước + 2h
    N'Đơn gọi mẫu cho bàn 3'
);

-- Đơn gọi 1 (DonGoiId = 1): 2 cà phê đen, 1 cà phê sữa
INSERT INTO ChiTietDonGoi
    (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
    (1, 1, NULL, 2, 20000.00, 0.00, N'2 ly cà phê đen'),
    (1, 2, NULL, 1, 25000.00, 0.00, N'1 ly cà phê sữa');

-- Đơn gọi 2 (DonGoiId = 2): 1 cà phê sữa, 1 bạc xỉu
INSERT INTO ChiTietDonGoi
    (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
    (2, 2, NULL, 1, 25000.00, 0.00, N'1 ly cà phê sữa'),
    (2, 3, NULL, 1, 30000.00, 0.00, N'1 ly bạc xỉu');

-- Đơn gọi 3 (DonGoiId = 3): 1 nước cam, 1 sinh tố xoài
INSERT INTO ChiTietDonGoi
    (DonGoiId, ThucUongId, ThucAnId, SoLuong, DonGia, ChietKhau, GhiChu)
VALUES
    (3, 4, NULL, 1, 30000.00, 0.00, N'1 ly nước cam'),
    (3, 5, NULL, 1, 35000.00, 0.00, N'1 ly sinh tố xoài');

    -- Hóa đơn cho DonGoi 1 (Bàn 1, NV001 – Id = 2)
INSERT INTO HoaDon
    (MaHoaDon, NhanVienId, BanId,
     CreatedAt,                ClosedAt,
     TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
    ('HD0001', 2, 1,
     DATEADD(MINUTE, -30, GETDATE()), GETDATE(),
     1,        65000.00, 0.00, 0.00, N'Hóa đơn mẫu cho bàn 1');

-- Hóa đơn cho DonGoi 2 (Bàn 2, NV001 – Id = 2, ngày hôm qua)
INSERT INTO HoaDon
    (MaHoaDon, NhanVienId, BanId,
     CreatedAt,                       ClosedAt,
     TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
    ('HD0002', 2, 2,
     DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, DATEADD(HOUR, 1, GETDATE())),
     1,        55000.00, 0.00, 0.00, N'Hóa đơn mẫu cho bàn 2');

-- Hóa đơn cho DonGoi 3 (Bàn 3, NV002 – Id = 3, 2 ngày trước)
INSERT INTO HoaDon
    (MaHoaDon, NhanVienId, BanId,
     CreatedAt,                       ClosedAt,
     TrangThai, TongTien, GiamGia, Thue, GhiChu)
VALUES
    ('HD0003', 3, 3,
     DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -2, DATEADD(HOUR, 1, GETDATE())),
     1,        65000.00, 0.00, 0.00, N'Hóa đơn mẫu cho bàn 3');


     -- Thanh toán DonGoi 1
INSERT INTO ThanhToan
    (DonGoiId, SoTien,   PhuongThuc,      ThanhToanLuc,        MaThamChieu)
VALUES
    (1,       65000.00, N'Tiền mặt',      GETDATE(),           'TT0001');

-- Thanh toán DonGoi 2 (hôm qua)
INSERT INTO ThanhToan
    (DonGoiId, SoTien,   PhuongThuc,      ThanhToanLuc,                      MaThamChieu)
VALUES
    (2,       55000.00, N'Chuyển khoản', DATEADD(DAY, -1, GETDATE()),       'TT0002');

-- Thanh toán DonGoi 3 (2 ngày trước)
INSERT INTO ThanhToan
    (DonGoiId, SoTien,   PhuongThuc,      ThanhToanLuc,                      MaThamChieu)
VALUES
    (3,       65000.00, N'Tiền mặt',      DATEADD(DAY, -2, GETDATE()),      'TT0003');




    select * from ThanhToan
    SELECT Id, DanhMucId, Ten, DonGia
FROM ThucUong;