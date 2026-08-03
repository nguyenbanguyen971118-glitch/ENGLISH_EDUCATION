use railway;

-- 1. hệ thống danh mục động 
create table nhomdanhmuc (
    manhom int primary key auto_increment,
    macode varchar(50) unique not null comment 'vd: loai_phong, trang_thai_khoa_hoc',
    tennhom varchar(255) not null,
    ghichu text,
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, 
    nguoisua char(36), thoigiansua datetime null on update current_timestamp, 
    trangthai tinyint(1) default 1, daxoa tinyint(1) default 0
);

create table chitietdanhmuc (
    machitiet int primary key auto_increment,
    manhom int not null,
    macode varchar(50) null comment 'vd: ph_lab, tkh_open',
    tenchitiet varchar(255) not null,
    thutu int default 0,
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, 
    nguoisua char(36), thoigiansua datetime null on update current_timestamp, 
    trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (manhom) references nhomdanhmuc(manhom)
);

-- 2. hệ thống phân quyền (dùng int)
create table chucnang (
    machucnang int primary key auto_increment,
    tenchucnang varchar(100) not null,
    mota varchar(255),
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0
);

create table quyen (
    maquyen int primary key auto_increment,
    machucnang int not null,
    tenquyen varchar(100) not null,
    mota varchar(255),
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (machucnang) references chucnang(machucnang)
);

create table vaitro (
    mavaitro int primary key auto_increment,
    tenvaitro varchar(50) not null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0
);

create table vaitroquyen (
    mavaitro int not null,
    maquyen int not null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (mavaitro, maquyen),
    foreign key (mavaitro) references vaitro(mavaitro),
    foreign key (maquyen) references quyen(maquyen)
);

 -- 3 quản lý người dùng (dùng char(36) cho guid)
create table nguoidung (
    manguoidung char(36) primary key comment 'guid cho tài khoản',
    loaitaikhoan tinyint not null comment '1: admin, 2: giangvien, 3: hocsinh, 4: phuhuynh',
    
    tendangnhap varchar(50) unique not null,
    matkhauhash varchar(255) not null,
    hoten varchar(100) not null,
    email varchar(100) unique,
    anhdaidien varchar(255),
    matrangthai int comment 'trỏ về chitietdanhmuc',
    tokenxacminh varchar(255) null,
    daxacminhemail tinyint(1) default 0,
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

create table nguoidungvaitro (
    manguoidung char(36) not null,
    mavaitro int not null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (manguoidung, mavaitro),
    foreign key (manguoidung) references nguoidung(manguoidung),
    foreign key (mavaitro) references vaitro(mavaitro)
);

create table giangvien ( 
    magiangvien char(36) primary key, 
    manguoidung char(36) unique not null, 
    
    sodienthoai varchar(15),
    quequan varchar(255),
    trinhdochuyenmon varchar(255) not null,
    hocvi varchar(100),
    kinhnghiemgiangday text,
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (manguoidung) references nguoidung(manguoidung) 
);

create table hocsinh ( 
    mahocsinh char(36) primary key, 
    manguoidung char(36) unique not null, 
    
    ngaysinh date,
    quequan varchar(255),
    sodienthoainguoithan varchar(15),
    truongdangtheohoc varchar(255),
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (manguoidung) references nguoidung(manguoidung) 
);

create table phuhuynh ( 
    maphuhuynh char(36) primary key, 
    manguoidung char(36) unique not null, 
    
    sodienthoai varchar(15) not null,
    diachilienhe text,
    nghenghiep varchar(255),
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (manguoidung) references nguoidung(manguoidung) 
);

create table phuhuynhhocsinh ( 
    maphuhuynh char(36) not null, 
    mahocsinh char(36) not null, 
    maquanhe int comment 'trỏ về chitietdanhmuc', 
    
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (maphuhuynh, mahocsinh),
    foreign key (maphuhuynh) references phuhuynh(maphuhuynh),
    foreign key (mahocsinh) references hocsinh(mahocsinh),
    foreign key (maquanhe) references chitietdanhmuc(machitiet)
);

-- 4 quản lý khóa học & lớp học
create table khoahoc (
    makhoahoc char(36) primary key,
    tenkhoahoc varchar(255) not null,
    mota text,
    giacoban decimal(14,2),
    matrangthai int comment 'trỏ về chitietdanhmuc',
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

create table chuonghoc (
    machuong char(36) primary key,
    makhoahoc char(36) not null,
    tenchuong varchar(255) not null,
    mota text,
    thutu int,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (makhoahoc) references khoahoc(makhoahoc)
);

create table tailieu (
    matailieu char(36) primary key,
    machuonghoc char(36) not null,
    tentailieu varchar(255) not null,
    linktailieu varchar(255),
    mota text,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (machuonghoc) references chuonghoc(machuong)
);

create table phonghoc (
    maphonghoc char(36) primary key,
    tenphong varchar(50) not null,
    succhua int,
    loaiphong int comment 'trỏ về chitietdanhmuc',
    link varchar(255),
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (loaiphong) references chitietdanhmuc(machitiet)
);

create table lophoc (
    malophoc char(36) primary key,
    tenlop varchar(100) not null,
    ngaybatdau date,
    ngayketthuc date,
    sisohientai int default 0,
    sisotoida int,
    matrangthai int comment 'trỏ về chitietdanhmuc',
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

create table chitietkhoahoc_lophoc ( 
    makhoahoc char(36) not null, 
    malophoc char(36) not null, 
    ghichu varchar(255), 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (makhoahoc, malophoc),
    foreign key (makhoahoc) references khoahoc(makhoahoc),
    foreign key (malophoc) references lophoc(malophoc)
);

create table giangvienlophoc ( 
    malophoc char(36) not null, 
    magiangvien char(36) not null, 
    loaivaitro tinyint check (loaivaitro in (1, 2)) default 1, 
    ngaythamgia date, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (malophoc, magiangvien),
    foreign key (malophoc) references lophoc(malophoc),
    foreign key (magiangvien) references giangvien(magiangvien)
);

create table hocsinhlophoc ( 
    malophoc char(36) not null, 
    mahocsinh char(36) not null, 
    matrangthai int comment 'trỏ về chitietdanhmuc', 
    ngaythamgia date, 
    ngayroilop date null, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (malophoc, mahocsinh),
    foreign key (malophoc) references lophoc(malophoc),
    foreign key (mahocsinh) references hocsinh(mahocsinh),
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

 -- 5 hoạt động lớp học & điểm danh
create table tiethoc (
    matiet int primary key auto_increment,
    tentiet varchar(50) not null,
    giobatdau time not null,
    gioketthuc time not null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0
);

create table buoihoc (
    mabuoihoc char(36) primary key,
    malophoc char(36) not null,
    maphonghoc char(36),
    ngayhoc date not null,
    matietbatdau int not null,
    matietketthuc int not null,
    tieude varchar(255),
    noidung text,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (malophoc) references lophoc(malophoc),
    foreign key (maphonghoc) references phonghoc(maphonghoc),
    foreign key (matietbatdau) references tiethoc(matiet),
    foreign key (matietketthuc) references tiethoc(matiet)
);

create table yeucaulichday (
    mayeucau char(36) not null,
    magiangvien char(36) not null,
    malophoc char(36) not null,
    mabuoihoc char(36) null,
    ngayhocdexuat date not null,
    matietbatdaudexuat int not null,
    matietketthucdexuat int not null,
    loaiyeucau tinyint null,
    maphonghocdexuat char(36) null,
    lydo text null,
    trangthaiduyet tinyint null,
    ghichuadmin text null,
    nguoitao char(36) null,
    thoigiantao datetime(6) null,
    nguoisua char(36) null,
    thoigiansua datetime(6) null,
    trangthai tinyint(1) null,
    daxoa tinyint(1) null,
    primary key (mayeucau),
    index idx_yeucau_giangvien (magiangvien),
    index idx_yeucau_lophoc (malophoc),
    index idx_yeucau_buoihoc (mabuoihoc),
    index idx_yeucau_phongdecuat (maphonghocdexuat)
);

create table diemdanh (
    mabuoihoc char(36) not null,
    mahocsinh char(36) not null,
    matrangthai int comment 'trỏ về chitietdanhmuc',
    ghichu varchar(255),
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (mabuoihoc, mahocsinh),
    foreign key (mabuoihoc) references buoihoc(mabuoihoc),
    foreign key (mahocsinh) references hocsinh(mahocsinh),
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

create table tainguyenluutru (
    matainguyen char(36) primary key,
    manguoidung char(36) not null,
    tentainguyen varchar(255) not null,
    link varchar(255) not null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (manguoidung) references nguoidung(manguoidung)
);

-- 6  bài tập & sự kiện lớp học
create table nganhangcauhoi (
    macauhoi char(36) primary key,
    makhoahoc char(36) not null,
    loaicauhoi int comment 'trỏ về chitietdanhmuc', 
    mucdo int comment 'trỏ về chitietdanhmuc', 
    mucdichsudung tinyint default 1, 
    noidungcauhoi text not null,
    giaithichdapan text,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (makhoahoc) references khoahoc(makhoahoc),
    foreign key (loaicauhoi) references chitietdanhmuc(machitiet),
    foreign key (mucdo) references chitietdanhmuc(machitiet),
    constraint chk_mucdichsudung check (mucdichsudung in (1, 2, 3))
);

create table dapan (
    madapan char(36) primary key,
    macauhoi char(36) not null,
    noidungdapan text not null,
    ladapandung tinyint(1) default 0,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (macauhoi) references nganhangcauhoi(macauhoi)
);

create table baitap (
    mabaitap char(36) primary key,
    makhoahoc char(36) not null,
    tenbaitap varchar(255) not null,
    mota text,
    loaibaitap int comment 'trỏ về chitietdanhmuc', 
    thoigianlambai int null, 
    diemtoida decimal(5,2),
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (makhoahoc) references khoahoc(makhoahoc),
    foreign key (loaibaitap) references chitietdanhmuc(machitiet)
);

create table baitapcauhoi ( 
    mabaitap char(36) not null, 
    macauhoi char(36) not null, 
    diemcuacau decimal(5,2), 
    thutu int, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (mabaitap, macauhoi),
    foreign key (mabaitap) references baitap(mabaitap),
    foreign key (macauhoi) references nganhangcauhoi(macauhoi)
);

create table sukienlophoc (
    masukien char(36) primary key,
    malophoc char(36) not null,
    manguoidung char(36) not null,
    dangsukien int comment 'trỏ về chitietdanhmuc',
    mabaitap char(36) null, 
    noidung text,
    hannop datetime,
    matrangthai int comment 'trỏ về chitietdanhmuc',
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (malophoc) references lophoc(malophoc),
    foreign key (manguoidung) references nguoidung(manguoidung),
    foreign key (dangsukien) references chitietdanhmuc(machitiet),
    foreign key (mabaitap) references baitap(mabaitap),
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

create table nguoinhansukien ( 
    masukien char(36) not null, 
    mahocsinh char(36) not null, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (masukien, mahocsinh),
    foreign key (masukien) references sukienlophoc(masukien),
    foreign key (mahocsinh) references hocsinh(mahocsinh)
);

create table dinhkem ( 
    masukien char(36) not null, 
    matainguyen char(36) not null, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (masukien, matainguyen),
    foreign key (masukien) references sukienlophoc(masukien),
    foreign key (matainguyen) references tainguyenluutru(matainguyen)
);

create table nopbai (
    manopbai char(36) primary key,
    masukien char(36) not null,
    mahocsinh char(36) not null,
    thoigianbatdau datetime null,
    thoigiannop datetime null,
    nhanxetgiaovien text,
    diemso decimal(5,2),
    lannop int default 1,
    matrangthai int comment 'trỏ về chitietdanhmuc',
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (masukien) references sukienlophoc(masukien),
    foreign key (mahocsinh) references hocsinh(mahocsinh),
    foreign key (matrangthai) references chitietdanhmuc(machitiet)
);

create table dinhkemnopbai ( 
    manopbai char(36) not null, 
    matainguyen char(36) not null, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (manopbai, matainguyen),
    foreign key (manopbai) references nopbai(manopbai),
    foreign key (matainguyen) references tainguyenluutru(matainguyen)
);

create table chitietnopbai ( 
    machitiet char(36) primary key, 
    manopbai char(36) not null, 
    macauhoi char(36) not null, 
    madapanchon char(36) null, 
    cautraloidienkhuyet text null, 
    diemdatduoc decimal(5,2) default 0,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (manopbai) references nopbai(manopbai),
    foreign key (macauhoi) references nganhangcauhoi(macauhoi),
    foreign key (madapanchon) references dapan(madapan)
);

create table binhluan (
    mabinhluan char(36) primary key,
    masukien char(36) not null,
    manguoidung char(36) not null,
    noidung text not null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (masukien) references sukienlophoc(masukien),
    foreign key (manguoidung) references nguoidung(manguoidung)
);

-- 7. giao tiếp & tiện ích khác
create table hoithoai ( 
    mahoithoai char(36) primary key, 
    tieude varchar(255) null,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0
);

create table thanhvienhoithoai ( 
    mahoithoai char(36) not null, 
    manguoidung char(36) not null, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (mahoithoai, manguoidung),
    foreign key (mahoithoai) references hoithoai(mahoithoai),
    foreign key (manguoidung) references nguoidung(manguoidung)
);

create table tinnhan ( 
    matinnhan char(36) primary key, 
    mahoithoai char(36) not null, 
    manguoidunggui char(36) not null, 
    noidung text not null, 
    dadoc tinyint(1) default 0,
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    foreign key (mahoithoai) references hoithoai(mahoithoai),
    foreign key (manguoidunggui) references nguoidung(manguoidung)
);

create table thongbao ( 
    mathongbao char(36) primary key, 
    tieude varchar(255) not null, 
    noidung text not null, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0
);

create table nguoinhanthongbao ( 
    mathongbao char(36) not null, 
    manguoidung char(36) not null, 
    dadoc tinyint(1) default 0, 
    ngaydoc datetime, 
    nguoitao char(36), thoigiantao datetime default current_timestamp, nguoisua char(36), thoigiansua datetime null on update current_timestamp, trangthai tinyint(1) default 1, daxoa tinyint(1) default 0,
    primary key (mathongbao, manguoidung),
    foreign key (mathongbao) references thongbao(mathongbao),
    foreign key (manguoidung) references nguoidung(manguoidung)
);
alter table thongbao 
add column doituong varchar(255) not null default 'tat_ca';
-- Thêm cột thời gian đăng nhập lần cuối (nếu thiếu)
ALTER TABLE nguoidung
ADD COLUMN LanCuoiDangNhap DATETIME NULL;
create table if not exists dinhkemthongbao (
    mathongbao char(36) not null,
    matainguyen char(36) not null,
    nguoitao char(36) null,
    thoigiantao datetime(6) null,
    trangthai tinyint(1) null,
    daxoa tinyint(1) null,
    primary key (mathongbao, matainguyen),
    index idx_dinhkem_thongbao (mathongbao),
    index idx_dinhkem_tainguyen (matainguyen)
);
CREATE TABLE IF NOT EXISTS giangvienkhoahoc (
    magiangvien char(36) not null,
    makhoahoc char(36) not null,
    nguoitao char(36) null,
    thoigiantao datetime default current_timestamp,
    primary key (magiangvien, makhoahoc),
    foreign key (magiangvien) references giangvien(magiangvien),
    foreign key (makhoahoc) references khoahoc(makhoahoc)
);
insert into tiethoc (matiet, tentiet, giobatdau, gioketthuc) values
(1, 'tiết 1', '07:00:00', '07:45:00'),
(2, 'tiết 2', '07:50:00', '08:35:00'),
(3, 'tiết 3', '08:45:00', '09:30:00'),
(4, 'tiết 4', '09:35:00', '10:20:00'),
(5, 'tiết 5', '10:25:00', '11:10:00'),
(6, 'tiết 6', '13:30:00', '14:15:00'),
(7, 'tiết 7', '14:20:00', '15:05:00'),
(8, 'tiết 8', '15:15:00', '16:00:00'),
(9, 'tiết 9', '16:05:00', '16:50:00'),
(10, 'tiết 10', '17:00:00', '17:45:00')
on duplicate key update tentiet = values(tentiet);
create index idx_vaitroquyen_quyen_active on vaitroquyen(maquyen, daxoa, trangthai);
create index idx_nguoidungvaitro_vaitro_active on nguoidungvaitro(mavaitro, daxoa, trangthai);
create index idx_hocsinhlophoc_hocsinh_active on hocsinhlophoc(mahocsinh, daxoa, trangthai);
create index idx_giangvienlophoc_giangvien_active on giangvienlophoc(magiangvien, daxoa, trangthai);
create index idx_chitietkhlh_lophoc_active on chitietkhoahoc_lophoc(malophoc, daxoa, trangthai);
create index idx_phuhuynhhocsinh_hocsinh_active on phuhuynhhocsinh(mahocsinh, daxoa, trangthai);
create index idx_baitapcauhoi_cauhoi_active on baitapcauhoi(macauhoi, daxoa, trangthai);
create index idx_diemdanh_hocsinh_active on diemdanh(mahocsinh, daxoa, trangthai);
create index idx_nguoinhansukien_hocsinh_active on nguoinhansukien(mahocsinh, daxoa, trangthai);
create index idx_dinhkem_tainguyen_active on dinhkem(matainguyen, daxoa, trangthai);
create index idx_dinhkemnopbai_tainguyen_active on dinhkemnopbai(matainguyen, daxoa, trangthai);
create index idx_chitietnopbai_cauhoi_active on chitietnopbai(macauhoi, daxoa, trangthai);
create index idx_thanhvienhoithoai_user_active on thanhvienhoithoai(manguoidung, daxoa, trangthai);
create index idx_nguoinhanthongbao_user_dadoc on nguoinhanthongbao(manguoidung, dadoc, daxoa);