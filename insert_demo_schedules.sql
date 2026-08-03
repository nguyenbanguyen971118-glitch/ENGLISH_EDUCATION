USE railway;

-- ==============================================================================
-- XÓA DỮ LIỆU CŨ (DỌN DẸP SẠCH TRƯỚC KHI NẠP DEMO)
-- ==============================================================================
set foreign_key_checks = 0;
truncate table yeucaulichday;
truncate table buoihoc;
truncate table giangvienlophoc;
truncate table hocsinhlophoc;
truncate table phuhuynhhocsinh;
truncate table nguoidungvaitro;
truncate table giangvien;
truncate table hocsinh;
truncate table phuhuynh;
truncate table nguoidung;
truncate table lophoc;
truncate table khoahoc;
truncate table phonghoc;
truncate table tiethoc;
set foreign_key_checks = 1;

-- ==============================================================================
-- THIẾT LẬP KHUNG GIỜ HỌC (14 TIẾT)
-- ==============================================================================
insert into tiethoc (matiet, tentiet, giobatdau, gioketthuc) values
(1, 'tiết 1', '07:00:00', '07:50:00'), (2, 'tiết 2', '07:55:00', '08:45:00'),
(3, 'tiết 3', '08:50:00', '09:40:00'), (4, 'tiết 4', '09:50:00', '10:40:00'),
(5, 'tiết 5', '10:45:00', '11:35:00'), (6, 'tiết 6', '12:30:00', '13:20:00'),
(7, 'tiết 7', '13:25:00', '14:15:00'), (8, 'tiết 8', '14:20:00', '15:10:00'),
(9, 'tiết 9', '15:20:00', '16:10:00'), (10, 'tiết 10', '16:15:00', '17:05:00'),
(11, 'tiết 11', '17:30:00', '18:20:00'), (12, 'tiết 12', '18:25:00', '19:15:00'),
(13, 'tiết 13', '19:20:00', '20:10:00'), (14, 'tiết 14', '20:15:00', '21:05:00');

-- ==============================================================================
-- TẠO CÁC KHÓA HỌC (CHUYÊN VỀ TIẾNG ANH)
-- ==============================================================================
set @kh_pt = uuid(), @kh_ielts = uuid();

insert into khoahoc (makhoahoc, tenkhoahoc, mota, giacoban, daxoa) values
(@kh_pt, 'tiếng anh phổ thông', 'chương trình tiếng anh bám sát sgk từ lớp 1-12', 5000000, 0),
(@kh_ielts, 'lộ trình ielts chuyên sâu', 'target 6.5 - 7.5+', 15000000, 0);

-- ==============================================================================
-- TẠO CÁC LỚP HỌC (LỚP 1 -> 12 VÀ CÁC LỚP IELTS)
-- ==============================================================================
set @c_l1=uuid(), @c_l2=uuid(), @c_l3=uuid(), @c_l4=uuid(), @c_l5=uuid(), @c_l6=uuid();
set @c_l7=uuid(), @c_l8=uuid(), @c_l9=uuid(), @c_l10=uuid(), @c_l11=uuid(), @c_l12=uuid();
set @c_i1=uuid(), @c_i2=uuid(), @c_i3=uuid();

insert into lophoc (malophoc, tenlop, sisotoida, daxoa) values
(@c_l1, 'tiếng anh lớp 1', 30, 0), (@c_l2, 'tiếng anh lớp 2', 30, 0), (@c_l3, 'tiếng anh lớp 3', 30, 0),
(@c_l4, 'tiếng anh lớp 4', 30, 0), (@c_l5, 'tiếng anh lớp 5', 30, 0), (@c_l6, 'tiếng anh lớp 6', 35, 0),
(@c_l7, 'tiếng anh lớp 7', 35, 0), (@c_l8, 'tiếng anh lớp 8', 35, 0), (@c_l9, 'tiếng anh lớp 9', 35, 0),
(@c_l10, 'tiếng anh lớp 10', 40, 0), (@c_l11, 'tiếng anh lớp 11', 40, 0), (@c_l12, 'tiếng anh lớp 12', 40, 0),
(@c_i1, 'ielts 5.0', 15, 0), (@c_i2, 'ielts 6.0', 15, 0), (@c_i3, 'ielts 7.0', 10, 0);

-- ==============================================================================
-- KHỞI TẠO BIẾN CHO 40 USERS (10 GV, 10 PH, 20 HS)
-- ==============================================================================
SET @u_gv1=UUID(), @u_gv2=UUID(), @u_gv3=UUID(), @u_gv4=UUID(), @u_gv5=UUID(), @u_gv6=UUID(), @u_gv7=UUID(), @u_gv8=UUID(), @u_gv9=UUID(), @u_gv10=UUID();
SET @gv1=UUID(), @gv2=UUID(), @gv3=UUID(), @gv4=UUID(), @gv5=UUID(), @gv6=UUID(), @gv7=UUID(), @gv8=UUID(), @gv9=UUID(), @gv10=UUID();

SET @u_ph1=UUID(), @u_ph2=UUID(), @u_ph3=UUID(), @u_ph4=UUID(), @u_ph5=UUID(), @u_ph6=UUID(), @u_ph7=UUID(), @u_ph8=UUID(), @u_ph9=UUID(), @u_ph10=UUID();
SET @ph1=UUID(), @ph2=UUID(), @ph3=UUID(), @ph4=UUID(), @ph5=UUID(), @ph6=UUID(), @ph7=UUID(), @ph8=UUID(), @ph9=UUID(), @ph10=UUID();

SET @u_hs1=UUID(), @u_hs2=UUID(), @u_hs3=UUID(), @u_hs4=UUID(), @u_hs5=UUID(), @u_hs6=UUID(), @u_hs7=UUID(), @u_hs8=UUID(), @u_hs9=UUID(), @u_hs10=UUID();
SET @u_hs11=UUID(), @u_hs12=UUID(), @u_hs13=UUID(), @u_hs14=UUID(), @u_hs15=UUID(), @u_hs16=UUID(), @u_hs17=UUID(), @u_hs18=UUID(), @u_hs19=UUID(), @u_hs20=UUID();
SET @hs1=UUID(), @hs2=UUID(), @hs3=UUID(), @hs4=UUID(), @hs5=UUID(), @hs6=UUID(), @hs7=UUID(), @hs8=UUID(), @hs9=UUID(), @hs10=UUID();
SET @hs11=UUID(), @hs12=UUID(), @hs13=UUID(), @hs14=UUID(), @hs15=UUID(), @hs16=UUID(), @hs17=UUID(), @hs18=UUID(), @hs19=UUID(), @hs20=UUID();

-- ==============================================================================
-- INSERT BẢNG NGUOIDUNG & VAI TRÒ
-- ==============================================================================
-- INSERT 10 Giảng Viên
insert into nguoidung (manguoidung, loaitaikhoan, tendangnhap, matkhauhash, hoten, email, daxoa) values
(@u_gv1, 2, 'gv_minhthu', 'hash', 'trần thị minh thu', 'minhthu@epu.edu.vn', 0),
(@u_gv2, 2, 'gv_hoanghai', 'hash', 'lê hoàng hải', 'hoanghai@epu.edu.vn', 0),
(@u_gv3, 2, 'gv_tuananh', 'hash', 'phạm tuấn anh', 'tuananh@epu.edu.vn', 0),
(@u_gv4, 2, 'gv_bichngoc', 'hash', 'vũ bích ngọc', 'bichngoc@epu.edu.vn', 0),
(@u_gv5, 2, 'gv_xuantruong', 'hash', 'đặng xuân trường', 'xuantruong@epu.edu.vn', 0),
(@u_gv6, 2, 'gv_thuha', 'hash', 'bùi thu hà', 'thuha@epu.edu.vn', 0),
(@u_gv7, 2, 'gv_quanghieu', 'hash', 'hồ quang hiếu', 'quanghieu@epu.edu.vn', 0),
(@u_gv8, 2, 'gv_phuongthao', 'hash', 'ngô phương thảo', 'phuongthao@epu.edu.vn', 0),
(@u_gv9, 2, 'gv_vanquyet', 'hash', 'trịnh văn quyết', 'vanquyet@epu.edu.vn', 0),
(@u_gv10, 2, 'gv_lananh', 'hash', 'nguyễn lan anh', 'lananh@epu.edu.vn', 0);

-- INSERT 10 Phụ Huynh
insert into nguoidung (manguoidung, loaitaikhoan, tendangnhap, matkhauhash, hoten, email, daxoa) values
(@u_ph1, 4, 'ph_daiquang', 'hash', 'trần đại quang', 'daiquang.tran@gmail.com', 0),
(@u_ph2, 4, 'ph_thimai', 'hash', 'nguyễn thị mai', 'thimai.nguyen@gmail.com', 0),
(@u_ph3, 4, 'ph_vanluan', 'hash', 'lê văn luân', 'vanluan.le@gmail.com', 0),
(@u_ph4, 4, 'ph_thuthuy', 'hash', 'phạm thu thủy', 'thuthuy.pham@gmail.com', 0),
(@u_ph5, 4, 'ph_anhtuan', 'hash', 'hoàng anh tuấn', 'anhtuan.hoang@gmail.com', 0),
(@u_ph6, 4, 'ph_thihong', 'hash', 'vũ thị hồng', 'thihong.vu@gmail.com', 0),
(@u_ph7, 4, 'ph_tridung', 'hash', 'đặng trí dũng', 'tridung.dang@gmail.com', 0),
(@u_ph8, 4, 'ph_vanhung', 'hash', 'bùi văn hùng', 'vanhung.bui@gmail.com', 0),
(@u_ph9, 4, 'ph_thituyet', 'hash', 'phan thị tuyết', 'thituyet.phan@gmail.com', 0),
(@u_ph10, 4, 'ph_minhkhang', 'hash', 'đỗ minh khang', 'minhkhang.do@gmail.com', 0);

-- INSERT 20 Học Sinh
insert into nguoidung (manguoidung, loaitaikhoan, tendangnhap, matkhauhash, hoten, email, daxoa) values
(@u_hs1, 3, 'hs_tranlam', 'hash', 'trần lâm', 'tranlam@gmail.com', 0),
(@u_hs2, 3, 'hs_tranlena', 'hash', 'trần lê na', 'tranlena@gmail.com', 0),
(@u_hs3, 3, 'hs_nguyenbach', 'hash', 'nguyễn hoàng bách', 'hoangbach@gmail.com', 0),
(@u_hs4, 3, 'hs_nguyenyen', 'hash', 'nguyễn thị yến', 'thiyen@gmail.com', 0),
(@u_hs5, 3, 'hs_lebaotram', 'hash', 'lê bảo trâm', 'baotram@gmail.com', 0),
(@u_hs6, 3, 'hs_leminhtriet', 'hash', 'lê minh triết', 'minhtriet@gmail.com', 0),
(@u_hs7, 3, 'hs_phamgiabao', 'hash', 'phạm gia bảo', 'giabao@gmail.com', 0),
(@u_hs8, 3, 'hs_phamhuonggiang', 'hash', 'phạm hương giang', 'huonggiang@gmail.com', 0),
(@u_hs9, 3, 'hs_hoangminhdat', 'hash', 'hoàng minh đạt', 'minhdat@gmail.com', 0),
(@u_hs10, 3, 'hs_hoangthutrang', 'hash', 'hoàng thu trang', 'thutrang@gmail.com', 0),
(@u_hs11, 3, 'hs_vutuankiet', 'hash', 'vũ tuấn kiệt', 'tuankiet@gmail.com', 0),
(@u_hs12, 3, 'hs_vuphuonganh', 'hash', 'vũ phương anh', 'phuonganh@gmail.com', 0),
(@u_hs13, 3, 'hs_dangquynhchi', 'hash', 'đặng quỳnh chi', 'quynhchi@gmail.com', 0),
(@u_hs14, 3, 'hs_danghaidang', 'hash', 'đặng hải đăng', 'haidang@gmail.com', 0),
(@u_hs15, 3, 'hs_buiducphat', 'hash', 'bùi đức phát', 'ducphat@gmail.com', 0),
(@u_hs16, 3, 'hs_buingocdiep', 'hash', 'bùi ngọc diệp', 'ngocdiep@gmail.com', 0),
(@u_hs17, 3, 'hs_phanbaolong', 'hash', 'phan bảo long', 'baolong@gmail.com', 0),
(@u_hs18, 3, 'hs_phanthaovy', 'hash', 'phan thảo vy', 'thaovy@gmail.com', 0),
(@u_hs19, 3, 'hs_doquangsang', 'hash', 'đỗ quang sang', 'quangsang@gmail.com', 0),
(@u_hs20, 3, 'hs_dogiahan', 'hash', 'đỗ gia hân', 'giahan@gmail.com', 0);

-- INSERT Quyền (NguoiDungVaiTro)
insert into nguoidungvaitro (manguoidung, mavaitro) values 
(@u_gv1, 2), (@u_gv2, 2), (@u_gv3, 2), (@u_gv4, 2), (@u_gv5, 2), (@u_gv6, 2), (@u_gv7, 2), (@u_gv8, 2), (@u_gv9, 2), (@u_gv10, 2),
(@u_ph1, 4), (@u_ph2, 4), (@u_ph3, 4), (@u_ph4, 4), (@u_ph5, 4), (@u_ph6, 4), (@u_ph7, 4), (@u_ph8, 4), (@u_ph9, 4), (@u_ph10, 4),
(@u_hs1, 3), (@u_hs2, 3), (@u_hs3, 3), (@u_hs4, 3), (@u_hs5, 3), (@u_hs6, 3), (@u_hs7, 3), (@u_hs8, 3), (@u_hs9, 3), (@u_hs10, 3),
(@u_hs11, 3), (@u_hs12, 3), (@u_hs13, 3), (@u_hs14, 3), (@u_hs15, 3), (@u_hs16, 3), (@u_hs17, 3), (@u_hs18, 3), (@u_hs19, 3), (@u_hs20, 3);

-- ==============================================================================
-- INSERT PROFILE (GIẢNG VIÊN, HỌC SINH, PHỤ HUYNH)
-- ==============================================================================
insert into giangvien (magiangvien, manguoidung, trinhdochuyenmon, daxoa) values
(@gv1, @u_gv1, 'sư phạm tiếng anh', 0), (@gv2, @u_gv2, 'cử nhân ngôn ngữ anh', 0), (@gv3, @u_gv3, 'thạc sĩ tesol', 0), (@gv4, @u_gv4, 'ielts 8.0, nghiệp vụ sư phạm', 0),
(@gv5, @u_gv5, 'sư phạm tiếng anh tiểu học', 0), (@gv6, @u_gv6, 'cử nhân ngôn ngữ anh', 0), (@gv7, @u_gv7, 'ielts 8.0', 0), (@gv8, @u_gv8, 'ielts 8.5', 0),
(@gv9, @u_gv9, 'thạc sĩ ngôn ngữ học', 0), (@gv10, @u_gv10, 'ielts 8.5, tesol', 0);

insert into phuhuynh (maphuhuynh, manguoidung, sodienthoai, daxoa) values
(@ph1, @u_ph1, '0901234567', 0), (@ph2, @u_ph2, '0988765432', 0), (@ph3, @u_ph3, '0912345678', 0), (@ph4, @u_ph4, '0933456789', 0),
(@ph5, @u_ph5, '0944567890', 0), (@ph6, @u_ph6, '0977890123', 0), (@ph7, @u_ph7, '0966789012', 0), (@ph8, @u_ph8, '0988901234', 0),
(@ph9, @u_ph9, '0999012345', 0), (@ph10, @u_ph10, '0900123456', 0);

insert into hocsinh (mahocsinh, manguoidung, truongdangtheohoc, daxoa) values
(@hs1, @u_hs1, 'th kim đồng', 0), (@hs2, @u_hs2, 'th kim đồng', 0), (@hs3, @u_hs3, 'th nguyễn du', 0), (@hs4, @u_hs4, 'th nguyễn du', 0),
(@hs5, @u_hs5, 'thcs cầu giấy', 0), (@hs6, @u_hs6, 'thcs cầu giấy', 0), (@hs7, @u_hs7, 'thcs trưng vương', 0), (@hs8, @u_hs8, 'thcs trưng vương', 0),
(@hs9, @u_hs9, 'thpt chu văn an', 0), (@hs10, @u_hs10, 'thpt chuyên sư phạm', 0), (@hs11, @u_hs11, 'thpt yên hòa', 0), (@hs12, @u_hs12, 'thpt kim liên', 0),
(@hs13, @u_hs13, 'thpt phan đình phùng', 0), (@hs14, @u_hs14, 'thpt việt đức', 0), (@hs15, @u_hs15, 'thpt trần phú', 0), (@hs16, @u_hs16, 'đh ngoại thương', 0),
(@hs17, @u_hs17, 'đh kinh tế quốc dân', 0), (@hs18, @u_hs18, 'đh bách khoa', 0), (@hs19, @u_hs19, 'đh quốc gia', 0), (@hs20, @u_hs20, 'đh fpt', 0);

-- map: 1 phụ huynh quản lý 2 học sinh (cùng họ)
insert into phuhuynhhocsinh (maphuhuynh, mahocsinh, maquanhe) values
(@ph1, @hs1, null), (@ph1, @hs2, null), (@ph2, @hs3, null), (@ph2, @hs4, null),
(@ph3, @hs5, null), (@ph3, @hs6, null), (@ph4, @hs7, null), (@ph4, @hs8, null),
(@ph5, @hs9, null), (@ph5, @hs10, null), (@ph6, @hs11, null), (@ph6, @hs12, null),
(@ph7, @hs13, null), (@ph7, @hs14, null), (@ph8, @hs15, null), (@ph8, @hs16, null),
(@ph9, @hs17, null), (@ph9, @hs18, null), (@ph10, @hs19, null), (@ph10, @hs20, null);

-- ==============================================================================
-- phân công giảng dạy và nhập học (lớp học)
-- ==============================================================================
insert into giangvienlophoc (malophoc, magiangvien, loaivaitro) values
(@c_l1, @gv5, 1), (@c_l2, @gv5, 1), (@c_l3, @gv5, 1), (@c_l4, @gv5, 1),
(@c_l5, @gv4, 1), (@c_l6, @gv2, 1), (@c_l7, @gv3, 1), (@c_l8, @gv1, 1),
(@c_l9, @gv9, 1), (@c_l10, @gv4, 1), (@c_l11, @gv2, 1), (@c_l12, @gv9, 1),
(@c_i1, @gv6, 1), (@c_i2, @gv7, 1), (@c_i3, @gv10, 1);

-- thêm học sinh vào các lớp
insert into hocsinhlophoc (malophoc, mahocsinh, ngaythamgia) values
(@c_l1, @hs1, curdate()), (@c_l1, @hs2, curdate()), (@c_l2, @hs3, curdate()), (@c_l2, @hs4, curdate()),
(@c_l5, @hs5, curdate()), (@c_l6, @hs6, curdate()), (@c_l7, @hs7, curdate()), (@c_l8, @hs8, curdate()),
(@c_l10, @hs9, curdate()), (@c_l11, @hs10, curdate()), (@c_l12, @hs11, curdate()), (@c_l12, @hs12, curdate()),
(@c_i1, @hs13, curdate()), (@c_i1, @hs14, curdate()), (@c_i2, @hs15, curdate()), (@c_i2, @hs16, curdate()),
(@c_i3, @hs17, curdate()), (@c_i3, @hs18, curdate()), (@c_i3, @hs19, curdate()), (@c_i3, @hs20, curdate());

-- ==============================================================================
-- tạo danh sách phòng học
-- ==============================================================================
set @r1=uuid(), @r2=uuid(), @r3=uuid(), @r4=uuid(), @r5=uuid();
set @r6=uuid(), @r7=uuid(), @r8=uuid(), @r9=uuid(), @r10=uuid();

insert into phonghoc (maphonghoc, tenphong, succhua, daxoa) values
(@r1, 'a101', 30, 0), (@r2, 'a102', 30, 0), (@r3, 'a103', 30, 0), (@r4, 'a104', 30, 0), (@r5, 'a105', 40, 0),
(@r6, 'a106', 40, 0), (@r7, 'a107', 40, 0), (@r8, 'a108', 20, 0), (@r9, 'a109', 20, 0), (@r10, 'a110', 50, 0);
