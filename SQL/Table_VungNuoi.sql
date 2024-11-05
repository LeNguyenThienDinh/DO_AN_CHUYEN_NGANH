CREATE TABLE USERS (
    USERNAME VARCHAR2(50) PRIMARY KEY,
    PASSWORD RAW(128) NOT NULL 
);
ALTER TABLE USERS ADD (ROLE VARCHAR2(20));
ALTER TABLE USERS ADD (KhachHangID VARCHAR2(10));
ALTER TABLE USERS ADD (LastSessionID VARCHAR2(255));

CREATE TABLE VungNuoi (
    VungNuoiID VARCHAR2(50) PRIMARY KEY,
    TenVung VARCHAR2(100) NOT NULL,
    DiaChi VARCHAR2(200),
    DienTich FLOAT,
    MoTa VARCHAR2(500)
);

CREATE TABLE HoNuoi (
    HoNuoiID VARCHAR2(50) PRIMARY KEY,
    TenHoNuoi VARCHAR2(100) NOT NULL,
    ChuHo VARCHAR2(100),
    SoDienThoai VARCHAR2(15),
    DiaChi VARCHAR2(200),
    VungNuoiID VARCHAR2(50),
    FOREIGN KEY (VungNuoiID) REFERENCES VungNuoi(VungNuoiID)
);

CREATE TABLE LoSanPham (
    MaLo VARCHAR2(50) PRIMARY KEY,
    NgayThuHoach DATE,
    SoLuong INT,
    TrangThai VARCHAR2(50),
    HoNuoiID VARCHAR2(50),
    DonViVanChuyenID VARCHAR2(50),
    FOREIGN KEY (HoNuoiID) REFERENCES HoNuoi(HoNuoiID)
);

CREATE TABLE SanPham (
    SanPhamID VARCHAR2(50) PRIMARY KEY,
    TenSanPham VARCHAR2(100) NOT NULL,
    MoTa VARCHAR2(500),
    Gia FLOAT,
    SoLuongTon INT,
    LoSanPhamID VARCHAR2(50),
    FOREIGN KEY (LoSanPhamID) REFERENCES LoSanPham(MaLo)
);

CREATE TABLE KhachHang (
    KhachHangID VARCHAR2(50) PRIMARY KEY,
    TenKhachHang VARCHAR2(100) NOT NULL,
    DiaChi VARCHAR2(200),
    SoDienThoai VARCHAR2(15),
    Email VARCHAR2(100)
);

CREATE TABLE NhanVien (
    NhanVienID VARCHAR2(50) PRIMARY KEY,
    TenNhanVien VARCHAR2(100) NOT NULL,
    ChucVu VARCHAR2(50),
    SoDienThoai VARCHAR2(15),
    NgaySinh DATE
);
alter table NhanVien add (DiaChi varchar2(20));
alter table NhanVien add (NoiSinh varchar2(20));

CREATE TABLE DonViVanChuyen (
    DonViVanChuyenID VARCHAR2(50) PRIMARY KEY,
    TenDonVi VARCHAR2(100) NOT NULL,
    SoDienThoai VARCHAR2(15),
    DiaChi VARCHAR2(200),
    TrangThai VARCHAR2(50)
);

CREATE TABLE HoaDon (
    HoaDonID VARCHAR2(50) PRIMARY KEY,
    NgayLap DATE,
    TongTien FLOAT,
    LoaiHoaDon VARCHAR2(50),
    KhachHangID VARCHAR2(50),
    NhanVienID VARCHAR2(50),
    FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID),
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID)
);

CREATE TABLE ChiTietHoaDon (
    ChiTietHoaDonID VARCHAR2(50) PRIMARY KEY,
    HoaDonID VARCHAR2(50),
    SanPhamID VARCHAR2(50),
    SoLuong INT,
    GiaTien FLOAT,
    ThanhTien FLOAT,
    KySo VARCHAR2(50),
    FOREIGN KEY (HoaDonID) REFERENCES HoaDon(HoaDonID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

CREATE TABLE DonHang (
    DonHangID VARCHAR2(50) PRIMARY KEY,
    NgayDat DATE,
    SoLuong INT,
    TongTien FLOAT,
    TrangThai VARCHAR2(50),
    KhachHangID VARCHAR2(50),
    NhanVienID VARCHAR2(50),
    DonViVanChuyenID VARCHAR2(50),
    FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID),
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID),
    FOREIGN KEY (DonViVanChuyenID) REFERENCES DonViVanChuyen(DonViVanChuyenID)
);

-- Ch?nh d? li?u b?ng VungNuoi
INSERT INTO VungNuoi (VungNuoiID, TenVung, DiaChi, DienTich, MoTa) VALUES
('V1', 'V?ng nu?i 1', '??a ch? 1', 1000.0, 'M? t? v? v?ng nu?i 1');
INSERT INTO VungNuoi (VungNuoiID, TenVung, DiaChi, DienTich, MoTa) VALUES
('V2', 'V?ng nu?i 2', '??a ch? 2', 1500.0, 'M? t? v? v?ng nu?i 2');

-- Ch?nh d? li?u b?ng HoNuoi
INSERT INTO HoNuoi (HoNuoiID, TenHoNuoi, ChuHo, SoDienThoai, DiaChi, VungNuoiID) VALUES
('H1', 'H? nu?i 1', 'Ch? h? 1', '0123456789', '??a ch? h? 1', 'V1');
INSERT INTO HoNuoi (HoNuoiID, TenHoNuoi, ChuHo, SoDienThoai, DiaChi, VungNuoiID) VALUES
('H2', 'H? nu?i 2', 'Ch? h? 2', '0987654321', '??a ch? h? 2', 'V2');

-- Ch?nh d? li?u b?ng LoSanPham
INSERT INTO LoSanPham (MaLo, NgayThuHoach, SoLuong, TrangThai, HoNuoiID, DonViVanChuyenID) VALUES
('L1', TO_DATE('2024-10-01', 'YYYY-MM-DD'), 200, 'C? h?ng', 'H1', 'DV1');
INSERT INTO LoSanPham (MaLo, NgayThuHoach, SoLuong, TrangThai, HoNuoiID, DonViVanChuyenID) VALUES
('L2', TO_DATE('2024-11-01', 'YYYY-MM-DD'), 150, 'C? h?ng', 'H2', 'DV2');

-- Ch?nh d? li?u b?ng SanPham
INSERT INTO SanPham (SanPhamID, TenSanPham, MoTa, Gia, SoLuongTon, LoSanPhamID) VALUES
('SP1', 'S?n ph?m 1', 'M? t? s?n ph?m 1', 100.0, 50, 'L1');
INSERT INTO SanPham (SanPhamID, TenSanPham, MoTa, Gia, SoLuongTon, LoSanPhamID) VALUES
('SP2', 'S?n ph?m 2', 'M? t? s?n ph?m 2', 150.0, 30, 'L2');

-- Ch?nh d? li?u b?ng KhachHang
INSERT INTO KhachHang (KhachHangID, TenKhachHang, DiaChi, SoDienThoai, Email) VALUES
('KH1', 'Kh?ch h?ng 1', '??a ch? kh?ch h?ng 1', '0112233445', 'khach1@example.com');
INSERT INTO KhachHang (KhachHangID, TenKhachHang, DiaChi, SoDienThoai, Email) VALUES
('KH2', 'Kh?ch h?ng 2', '??a ch? kh?ch h?ng 2', '0223344556', 'khach2@example.com');

-- Ch?nh d? li?u b?ng NhanVien
INSERT INTO NhanVien (NhanVienID, TenNhanVien, ChucVu, SoDienThoai, NgaySinh) VALUES
('NV1', 'Nh?n vi?n 1', 'Qu?n l?', '0334455667', TO_DATE('1990-01-15', 'YYYY-MM-DD'));
INSERT INTO NhanVien (NhanVienID, TenNhanVien, ChucVu, SoDienThoai, NgaySinh) VALUES
('NV2', 'Nh?n vi?n 2', 'Nh?n vi?n b?n h?ng', '0445566778', TO_DATE('1988-05-20', 'YYYY-MM-DD'));

-- Ch?nh d? li?u b?ng DonViVanChuyen
INSERT INTO DonViVanChuyen (DonViVanChuyenID, TenDonVi, SoDienThoai, DiaChi, TrangThai) VALUES
('DV1', '??n v? v?n chuy?n 1', '0556677889', '??a ch? DV 1', 'Ho?t ??ng');
INSERT INTO DonViVanChuyen (DonViVanChuyenID, TenDonVi, SoDienThoai, DiaChi, TrangThai) VALUES
('DV2', '??n v? v?n chuy?n 2', '0667788990', '??a ch? DV 2', 'Ng?ng ho?t ??ng');

-- Ch?nh d? li?u b?ng HoaDon
INSERT INTO HoaDon (HoaDonID, NgayLap, TongTien, LoaiHoaDon, KhachHangID, NhanVienID) VALUES
('HD1', TO_DATE('2024-10-15', 'YYYY-MM-DD'), 25000.0, 'H?a ??n b?n h?ng', 'KH1', 'NV1');
INSERT INTO HoaDon (HoaDonID, NgayLap, TongTien, LoaiHoaDon, KhachHangID, NhanVienID) VALUES
('HD2', TO_DATE('2024-10-20', 'YYYY-MM-DD'), 15000.0, 'H?a ??n b?n h?ng', 'KH2', 'NV2');

-- Ch?nh d? li?u b?ng ChiTietHoaDon
INSERT INTO ChiTietHoaDon (ChiTietHoaDonID, HoaDonID, SanPhamID, SoLuong, GiaTien, ThanhTien, KySo) VALUES
('CTHD1', 'HD1', 'SP1', 1, 100.0, 100.0, 'K? s? 1');
INSERT INTO ChiTietHoaDon (ChiTietHoaDonID, HoaDonID, SanPhamID, SoLuong, GiaTien, ThanhTien, KySo) VALUES
('CTHD2', 'HD1', 'SP2', 2, 150.0, 300.0, 'K? s? 1');
INSERT INTO ChiTietHoaDon (ChiTietHoaDonID, HoaDonID, SanPhamID, SoLuong, GiaTien, ThanhTien, KySo) VALUES
('CTHD3', 'HD2', 'SP2', 1, 150.0, 150.0, 'K? s? 2');

-- Ch?nh d? li?u b?ng DonHang
INSERT INTO DonHang (DonHangID, NgayDat, SoLuong, TongTien, TrangThai, KhachHangID, NhanVienID, DonViVanChuyenID) VALUES
('DH1', TO_DATE('2024-10-10', 'YYYY-MM-DD'), 2, 300.0, '?? giao', 'KH1', 'NV1', 'DV1');
INSERT INTO DonHang (DonHangID, NgayDat, SoLuong, TongTien, TrangThai, KhachHangID, NhanVienID, DonViVanChuyenID) VALUES
('DH2', TO_DATE('2024-10-12', 'YYYY-MM-DD'), 1, 150.0, '?ang giao', 'KH2', 'NV2', 'DV2');



