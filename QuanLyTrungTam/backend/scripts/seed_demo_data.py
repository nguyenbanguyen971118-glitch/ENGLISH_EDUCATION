#!/usr/bin/env python3
"""
Seeding Demo Data Script
Inserts comprehensive demo data for the QuanLyTrungTam system
"""

import pymysql
from datetime import datetime, timedelta
import uuid

# Database configuration
DB_HOST = '127.0.0.1'
DB_PORT = 3306
DB_USER = 'root'
DB_PASSWORD = ''  # Empty password
DB_NAME = 'HeThongHocOnline_Final'

def get_connection():
    """Create database connection"""
    return pymysql.connect(
        host=DB_HOST,
        port=DB_PORT,
        user=DB_USER,
        password=DB_PASSWORD,
        database=DB_NAME,
        charset='utf8mb4',
        cursorclass=pymysql.cursors.DictCursor
    )

def clear_tables(cursor):
    """Clear existing data"""
    print("🗑️  Clearing existing data...")
    cursor.execute("SET FOREIGN_KEY_CHECKS = 0")
    tables = [
        'YeuCauLichDay', 'BuoiHoc', 'GiangVienLopHoc', 'HocSinhLopHoc',
        'PhuHuynhHocSinh', 'NguoiDungVaiTro', 'GiangVien', 'HocSinh', 'PhuHuynh',
        'NguoiDung', 'LopHoc', 'KhoaHoc', 'PhongHoc', 'TietHoc'
    ]
    for table in tables:
        cursor.execute(f"TRUNCATE TABLE {table}")
    cursor.execute("SET FOREIGN_KEY_CHECKS = 1")
    print("✓ Data cleared")

def seed_tiet_hoc(cursor):
    """Insert time slots"""
    print("📚 Inserting time slots (TietHoc)...")
    slots = [
        (1, 'Tiết 1', '07:00:00', '07:50:00'),
        (2, 'Tiết 2', '07:55:00', '08:45:00'),
        (3, 'Tiết 3', '08:50:00', '09:40:00'),
        (4, 'Tiết 4', '09:50:00', '10:40:00'),
        (5, 'Tiết 5', '10:45:00', '11:35:00'),
        (6, 'Tiết 6', '12:30:00', '13:20:00'),
        (7, 'Tiết 7', '13:25:00', '14:15:00'),
        (8, 'Tiết 8', '14:20:00', '15:10:00'),
        (9, 'Tiết 9', '15:20:00', '16:10:00'),
        (10, 'Tiết 10', '16:15:00', '17:05:00'),
        (11, 'Tiết 11', '17:30:00', '18:20:00'),
        (12, 'Tiết 12', '18:25:00', '19:15:00'),
        (13, 'Tiết 13', '19:20:00', '20:10:00'),
        (14, 'Tiết 14', '20:15:00', '21:05:00'),
    ]
    
    for slot in slots:
        cursor.execute(
            "INSERT INTO TietHoc (MaTiet, TenTiet, GioBatDau, GioKetThuc) VALUES (%s, %s, %s, %s)",
            slot
        )
    print(f"✓ Inserted {len(slots)} time slots")

def seed_khoa_hoc(cursor):
    """Insert courses"""
    print("🎓 Inserting courses (KhoaHoc)...")
    global kh_pt, kh_ielts
    kh_pt = str(uuid.uuid4())
    kh_ielts = str(uuid.uuid4())
    
    courses = [
        (kh_pt, 'Tiếng Anh Phổ Thông', 'Chương trình Tiếng Anh bám sát SGK từ lớp 1-12', 5000000, 0),
        (kh_ielts, 'Lộ trình IELTS chuyên sâu', 'Target 6.5 - 7.5+', 15000000, 0),
    ]
    
    for course in courses:
        cursor.execute(
            "INSERT INTO KhoaHoc (MaKhoaHoc, TenKhoaHoc, MoTa, GiaCoBan, DaXoa) VALUES (%s, %s, %s, %s, %s)",
            course
        )
    print(f"✓ Inserted {len(courses)} courses")

def seed_lop_hoc(cursor):
    """Insert classes"""
    print("🏫 Inserting classes (LopHoc)...")
    global classes
    classes = {}
    class_data = [
        ('c_l1', 'Tiếng Anh Lớp 1', 30),
        ('c_l2', 'Tiếng Anh Lớp 2', 30),
        ('c_l3', 'Tiếng Anh Lớp 3', 30),
        ('c_l4', 'Tiếng Anh Lớp 4', 30),
        ('c_l5', 'Tiếng Anh Lớp 5', 30),
        ('c_l6', 'Tiếng Anh Lớp 6', 35),
        ('c_l7', 'Tiếng Anh Lớp 7', 35),
        ('c_l8', 'Tiếng Anh Lớp 8', 35),
        ('c_l9', 'Tiếng Anh Lớp 9', 35),
        ('c_l10', 'Tiếng Anh Lớp 10', 40),
        ('c_l11', 'Tiếng Anh Lớp 11', 40),
        ('c_l12', 'Tiếng Anh Lớp 12', 40),
        ('c_i1', 'IELTS 5.0', 15),
        ('c_i2', 'IELTS 6.0', 15),
        ('c_i3', 'IELTS 7.0', 10),
    ]
    
    for key, name, size in class_data:
        ma_lop = str(uuid.uuid4())
        classes[key] = ma_lop
        cursor.execute(
            "INSERT INTO LopHoc (MaLopHoc, TenLop, SiSoToiDa, DaXoa) VALUES (%s, %s, %s, %s)",
            (ma_lop, name, size, 0)
        )
    print(f"✓ Inserted {len(class_data)} classes")

def seed_users_and_profiles(cursor):
    """Insert users and their profiles"""
    print("👥 Inserting users and profiles...")
    global users, teachers, parents, students
    
    users = {}
    teachers = {}
    parents = {}
    students = {}
    
    # Teachers
    teacher_data = [
        ('gv1', 'gv_minhthu', 'Trần Thị Minh Thu', 'minhthu@epu.edu.vn', 'Sư phạm Tiếng Anh'),
        ('gv2', 'gv_hoanghai', 'Lê Hoàng Hải', 'hoanghai@epu.edu.vn', 'Cử nhân Ngôn ngữ Anh'),
        ('gv3', 'gv_tuananh', 'Phạm Tuấn Anh', 'tuananh@epu.edu.vn', 'Thạc sĩ TESOL'),
        ('gv4', 'gv_bichngoc', 'Vũ Bích Ngọc', 'bichngoc@epu.edu.vn', 'IELTS 8.0, Nghiệp vụ Sư phạm'),
        ('gv5', 'gv_xuantruong', 'Đặng Xuân Trường', 'xuantruong@epu.edu.vn', 'Sư phạm Tiếng Anh Tiểu học'),
        ('gv6', 'gv_thuha', 'Bùi Thu Hà', 'thuha@epu.edu.vn', 'Cử nhân Ngôn ngữ Anh'),
        ('gv7', 'gv_quanghieu', 'Hồ Quang Hiếu', 'quanghieu@epu.edu.vn', 'IELTS 8.0'),
        ('gv8', 'gv_phuongthao', 'Ngô Phương Thảo', 'phuongthao@epu.edu.vn', 'IELTS 8.5'),
        ('gv9', 'gv_vanquyet', 'Trịnh Văn Quyết', 'vanquyet@epu.edu.vn', 'Thạc sĩ Ngôn ngữ học'),
        ('gv10', 'gv_lananh', 'Nguyễn Lan Anh', 'lananh@epu.edu.vn', 'IELTS 8.5, TESOL'),
    ]
    
    for key, username, fullname, email, specialization in teacher_data:
        user_id = str(uuid.uuid4())
        teacher_id = str(uuid.uuid4())
        users[key] = user_id
        teachers[key] = teacher_id
        
        cursor.execute(
            "INSERT INTO NguoiDung (MaNguoiDung, LoaiTaiKhoan, TenDangNhap, MatKhauHash, HoTen, Email, DaXoa) VALUES (%s, %s, %s, %s, %s, %s, %s)",
            (user_id, 2, username, 'hash', fullname, email, 0)
        )
        cursor.execute(
            "INSERT INTO GiangVien (MaGiangVien, MaNguoiDung, TrinhDoChuyenMon, DaXoa) VALUES (%s, %s, %s, %s)",
            (teacher_id, user_id, specialization, 0)
        )
        cursor.execute(
            "INSERT INTO NguoiDungVaiTro (MaNguoiDung, MaVaiTro) VALUES (%s, %s)",
            (user_id, 2)
        )
    
    # Parents
    parent_data = [
        ('ph1', 'ph_daiquang', 'Trần Đại Quang', 'daiquang.tran@gmail.com', '0901234567'),
        ('ph2', 'ph_thimai', 'Nguyễn Thị Mai', 'thimai.nguyen@gmail.com', '0988765432'),
        ('ph3', 'ph_vanluan', 'Lê Văn Luân', 'vanluan.le@gmail.com', '0912345678'),
        ('ph4', 'ph_thuthuy', 'Phạm Thu Thủy', 'thuthuy.pham@gmail.com', '0933456789'),
        ('ph5', 'ph_anhtuan', 'Hoàng Anh Tuấn', 'anhtuan.hoang@gmail.com', '0944567890'),
        ('ph6', 'ph_thihong', 'Vũ Thị Hồng', 'thihong.vu@gmail.com', '0977890123'),
        ('ph7', 'ph_tridung', 'Đặng Trí Dũng', 'tridung.dang@gmail.com', '0966789012'),
        ('ph8', 'ph_vanhung', 'Bùi Văn Hùng', 'vanhung.bui@gmail.com', '0988901234'),
        ('ph9', 'ph_thituyet', 'Phan Thị Tuyết', 'thituyet.phan@gmail.com', '0999012345'),
        ('ph10', 'ph_minhkhang', 'Đỗ Minh Khang', 'minhkhang.do@gmail.com', '0900123456'),
    ]
    
    for key, username, fullname, email, phone in parent_data:
        user_id = str(uuid.uuid4())
        parent_id = str(uuid.uuid4())
        users[key] = user_id
        parents[key] = parent_id
        
        cursor.execute(
            "INSERT INTO NguoiDung (MaNguoiDung, LoaiTaiKhoan, TenDangNhap, MatKhauHash, HoTen, Email, DaXoa) VALUES (%s, %s, %s, %s, %s, %s, %s)",
            (user_id, 4, username, 'hash', fullname, email, 0)
        )
        cursor.execute(
            "INSERT INTO PhuHuynh (MaPhuHuynh, MaNguoiDung, SoDienThoai, DaXoa) VALUES (%s, %s, %s, %s)",
            (parent_id, user_id, phone, 0)
        )
        cursor.execute(
            "INSERT INTO NguoiDungVaiTro (MaNguoiDung, MaVaiTro) VALUES (%s, %s)",
            (user_id, 4)
        )
    
    # Students
    student_data = [
        ('hs1', 'hs_tranlam', 'Trần Lâm', 'tranlam@gmail.com', 'TH Kim Đồng'),
        ('hs2', 'hs_tranlena', 'Trần Lê Na', 'tranlena@gmail.com', 'TH Kim Đồng'),
        ('hs3', 'hs_nguyenbach', 'Nguyễn Hoàng Bách', 'hoangbach@gmail.com', 'TH Nguyễn Du'),
        ('hs4', 'hs_nguyenyen', 'Nguyễn Thị Yến', 'thiyen@gmail.com', 'TH Nguyễn Du'),
        ('hs5', 'hs_lebaotram', 'Lê Bảo Trâm', 'baotram@gmail.com', 'THCS Cầu Giấy'),
        ('hs6', 'hs_leminhtriet', 'Lê Minh Triết', 'minhtriet@gmail.com', 'THCS Cầu Giấy'),
        ('hs7', 'hs_phamgiabao', 'Phạm Gia Bảo', 'giabao@gmail.com', 'THCS Trưng Vương'),
        ('hs8', 'hs_phamhuonggiang', 'Phạm Hương Giang', 'huonggiang@gmail.com', 'THCS Trưng Vương'),
        ('hs9', 'hs_hoangminhdat', 'Hoàng Minh Đạt', 'minhdat@gmail.com', 'THPT Chu Văn An'),
        ('hs10', 'hs_hoangthutrang', 'Hoàng Thu Trang', 'thutrang@gmail.com', 'THPT Chuyên Sư Phạm'),
        ('hs11', 'hs_vutuankiet', 'Vũ Tuấn Kiệt', 'tuankiet@gmail.com', 'THPT Yên Hòa'),
        ('hs12', 'hs_vuphuonganh', 'Vũ Phương Anh', 'phuonganh@gmail.com', 'THPT Kim Liên'),
        ('hs13', 'hs_dangquynhchi', 'Đặng Quỳnh Chi', 'quynhchi@gmail.com', 'THPT Phan Đình Phùng'),
        ('hs14', 'hs_danghaidang', 'Đặng Hải Đăng', 'haidang@gmail.com', 'THPT Việt Đức'),
        ('hs15', 'hs_buiducphat', 'Bùi Đức Phát', 'ducphat@gmail.com', 'THPT Trần Phú'),
        ('hs16', 'hs_buingocdiep', 'Bùi Ngọc Diệp', 'ngocdiep@gmail.com', 'ĐH Ngoại Thương'),
        ('hs17', 'hs_phanbaolong', 'Phan Bảo Long', 'baolong@gmail.com', 'ĐH Kinh tế Quốc dân'),
        ('hs18', 'hs_phanthaovy', 'Phan Thảo Vy', 'thaovy@gmail.com', 'ĐH Bách Khoa'),
        ('hs19', 'hs_doquangsang', 'Đỗ Quang Sang', 'quangsang@gmail.com', 'ĐH Quốc Gia'),
        ('hs20', 'hs_dogiahan', 'Đỗ Gia Hân', 'giahan@gmail.com', 'ĐH FPT'),
    ]
    
    for key, username, fullname, email, school in student_data:
        user_id = str(uuid.uuid4())
        student_id = str(uuid.uuid4())
        users[key] = user_id
        students[key] = student_id
        
        cursor.execute(
            "INSERT INTO NguoiDung (MaNguoiDung, LoaiTaiKhoan, TenDangNhap, MatKhauHash, HoTen, Email, DaXoa) VALUES (%s, %s, %s, %s, %s, %s, %s)",
            (user_id, 3, username, 'hash', fullname, email, 0)
        )
        cursor.execute(
            "INSERT INTO HocSinh (MaHocSinh, MaNguoiDung, TruongDangTheoHoc, DaXoa) VALUES (%s, %s, %s, %s)",
            (student_id, user_id, school, 0)
        )
        cursor.execute(
            "INSERT INTO NguoiDungVaiTro (MaNguoiDung, MaVaiTro) VALUES (%s, %s)",
            (user_id, 3)
        )
    
    print(f"✓ Inserted {len(teacher_data)} teachers, {len(parent_data)} parents, {len(student_data)} students")

def seed_relationships(cursor):
    """Insert teacher-class, student-class, and parent-student relationships"""
    print("🔗 Inserting relationships...")
    
    # Teacher-Class assignments
    teacher_assignments = [
        ('gv5', ['c_l1', 'c_l2', 'c_l3', 'c_l4']),
        ('gv4', ['c_l5', 'c_l10']),
        ('gv2', ['c_l6', 'c_l11']),
        ('gv3', ['c_l7']),
        ('gv1', ['c_l8']),
        ('gv9', ['c_l9', 'c_l12']),
        ('gv6', ['c_i1']),
        ('gv7', ['c_i2']),
        ('gv10', ['c_i3']),
    ]
    
    for teacher_key, class_keys in teacher_assignments:
        teacher_id = teachers[teacher_key]
        for class_key in class_keys:
            cursor.execute(
                "INSERT INTO GiangVienLopHoc (MaLopHoc, MaGiangVien, LoaiVaiTro) VALUES (%s, %s, %s)",
                (classes[class_key], teacher_id, 1)
            )
    
    # Student-Class enrollment
    student_class_mapping = [
        ('hs1', 'c_l1'), ('hs2', 'c_l1'),
        ('hs3', 'c_l2'), ('hs4', 'c_l2'),
        ('hs5', 'c_l5'), ('hs6', 'c_l6'), ('hs7', 'c_l7'), ('hs8', 'c_l8'),
        ('hs9', 'c_l10'), ('hs10', 'c_l11'), ('hs11', 'c_l12'), ('hs12', 'c_l12'),
        ('hs13', 'c_i1'), ('hs14', 'c_i1'),
        ('hs15', 'c_i2'), ('hs16', 'c_i2'),
        ('hs17', 'c_i3'), ('hs18', 'c_i3'), ('hs19', 'c_i3'), ('hs20', 'c_i3'),
    ]
    
    for student_key, class_key in student_class_mapping:
        cursor.execute(
            "INSERT INTO HocSinhLopHoc (MaLopHoc, MaHocSinh, NgayThamGia) VALUES (%s, %s, %s)",
            (classes[class_key], students[student_key], datetime.now().date())
        )
    
    # Parent-Student relationships
    parent_student_mapping = [
        ('ph1', ['hs1', 'hs2']),
        ('ph2', ['hs3', 'hs4']),
        ('ph3', ['hs5', 'hs6']),
        ('ph4', ['hs7', 'hs8']),
        ('ph5', ['hs9', 'hs10']),
        ('ph6', ['hs11', 'hs12']),
        ('ph7', ['hs13', 'hs14']),
        ('ph8', ['hs15', 'hs16']),
        ('ph9', ['hs17', 'hs18']),
        ('ph10', ['hs19', 'hs20']),
    ]
    
    for parent_key, student_keys in parent_student_mapping:
        parent_id = parents[parent_key]
        for student_key in student_keys:
            cursor.execute(
                "INSERT INTO PhuHuynhHocSinh (MaPhuHuynh, MaHocSinh, MaQuanHe) VALUES (%s, %s, %s)",
                (parent_id, students[student_key], None)
            )
    
    print(f"✓ Inserted {len(teacher_assignments)} teacher assignments, {len(student_class_mapping)} student enrollments")

def seed_phong_hoc(cursor):
    """Insert classrooms"""
    print("🏛️  Inserting classrooms (PhongHoc)...")
    global rooms
    rooms = {}
    room_data = [
        ('r1', 'A101', 30),
        ('r2', 'A102', 30),
        ('r3', 'A103', 30),
        ('r4', 'A104', 30),
        ('r5', 'A105', 40),
        ('r6', 'A106', 40),
        ('r7', 'A107', 40),
        ('r8', 'A108', 20),
        ('r9', 'A109', 20),
        ('r10', 'A110', 50),
    ]
    
    for key, name, capacity in room_data:
        room_id = str(uuid.uuid4())
        rooms[key] = room_id
        cursor.execute(
            "INSERT INTO PhongHoc (MaPhongHoc, TenPhong, SucChua, DaXoa) VALUES (%s, %s, %s, %s)",
            (room_id, name, capacity, 0)
        )
    print(f"✓ Inserted {len(room_data)} classrooms")

def seed_buoi_hoc(cursor):
    """Insert schedule sessions"""
    print("📅 Inserting schedule sessions (BuoiHoc)...")
    
    # Get Monday of current week
    today = datetime.now().date()
    monday = today - timedelta(days=today.weekday())
    
    # Define schedule for all classes
    schedules = [
        # Class 1 & 2 (GV Trần Thị Minh Thu)
        ('c_l1', 'r1', [
            (monday, 2, 4, 'Tiếng Anh Lớp 1 - Từ vựng cơ bản'),
            (monday + timedelta(days=2), 2, 4, 'Tiếng Anh Lớp 1 - Cách phát âm chữ cái'),
            (monday + timedelta(days=7), 2, 4, 'Tiếng Anh Lớp 1 - Tập viết chữ cái'),
            (monday + timedelta(days=9), 2, 4, 'Tiếng Anh Lớp 1 - Bài kiểm tra tuần'),
        ]),
        ('c_l2', 'r2', [
            (monday + timedelta(days=1), 2, 4, 'Tiếng Anh Lớp 2 - Giới thiệu con vật'),
            (monday + timedelta(days=3), 2, 4, 'Tiếng Anh Lớp 2 - Các loại thực phẩm'),
            (monday + timedelta(days=8), 2, 4, 'Tiếng Anh Lớp 2 - Bài tập luyện tập'),
            (monday + timedelta(days=10), 2, 4, 'Tiếng Anh Lớp 2 - Cuối cùng tuần'),
        ]),
        
        # Class 3 & 4 (GV Trần Thị Minh Thu)
        ('c_l3', 'r3', [
            (monday + timedelta(days=1), 5, 7, 'Tiếng Anh Lớp 3 - Động từ cơ bản'),
            (monday + timedelta(days=3), 5, 7, 'Tiếng Anh Lớp 3 - Thì hiện tại đơn'),
            (monday + timedelta(days=8), 5, 7, 'Tiếng Anh Lớp 3 - Câu hỏi & Trả lời'),
            (monday + timedelta(days=10), 5, 7, 'Tiếng Anh Lớp 3 - Bài tập tổng hợp'),
        ]),
        ('c_l4', 'r4', [
            (monday + timedelta(days=2), 5, 7, 'Tiếng Anh Lớp 4 - Các tenses cơ bản'),
            (monday + timedelta(days=4), 5, 7, 'Tiếng Anh Lớp 4 - Modal verbs'),
            (monday + timedelta(days=9), 5, 7, 'Tiếng Anh Lớp 4 - Câu gián tiếp'),
            (monday + timedelta(days=11), 5, 7, 'Tiếng Anh Lớp 4 - Kiểm tra định kỳ'),
        ]),
        
        # Class 5 (GV Vũ Bích Ngọc)
        ('c_l5', 'r5', [
            (monday + timedelta(days=2), 9, 11, 'Tiếng Anh Lớp 5 - Từ vựng chủ đề'),
            (monday + timedelta(days=4), 9, 11, 'Tiếng Anh Lớp 5 - Phát triển kỹ năng đọc'),
            (monday + timedelta(days=9), 9, 11, 'Tiếng Anh Lớp 5 - Kỹ năng viết'),
            (monday + timedelta(days=11), 9, 11, 'Tiếng Anh Lớp 5 - Ôn tập tổng kết'),
        ]),
        
        # Class 6 (GV Lê Hoàng Hải)
        ('c_l6', 'r6', [
            (monday, 11, 13, 'Tiếng Anh Lớp 6 - Cấu trúc ngữ pháp'),
            (monday + timedelta(days=2), 11, 13, 'Tiếng Anh Lớp 6 - Bài đọc hiểu'),
            (monday + timedelta(days=7), 11, 13, 'Tiếng Anh Lớp 6 - Listening practice'),
            (monday + timedelta(days=9), 11, 13, 'Tiếng Anh Lớp 6 - Speaking exercise'),
        ]),
        
        # Class 7 (GV Phạm Tuấn Anh)
        ('c_l7', 'r7', [
            (monday + timedelta(days=1), 6, 8, 'Tiếng Anh Lớp 7 - Present Perfect'),
            (monday + timedelta(days=3), 6, 8, 'Tiếng Anh Lớp 7 - Past tenses'),
            (monday + timedelta(days=8), 6, 8, 'Tiếng Anh Lớp 7 - Conditional sentences'),
            (monday + timedelta(days=10), 6, 8, 'Tiếng Anh Lớp 7 - Ôn tập kiểm tra'),
        ]),
        
        # Class 8 (GV Trần Thị Minh Thu)
        ('c_l8', 'r8', [
            (monday + timedelta(days=2), 7, 9, 'Tiếng Anh Lớp 8 - Reported speech'),
            (monday + timedelta(days=4), 7, 9, 'Tiếng Anh Lớp 8 - Passive voice'),
            (monday + timedelta(days=9), 7, 9, 'Tiếng Anh Lớp 8 - Viết luận'),
            (monday + timedelta(days=11), 7, 9, 'Tiếng Anh Lớp 8 - Luyện tập tổng hợp'),
        ]),
        
        # Class 9 (GV Trịnh Văn Quyết)
        ('c_l9', 'r9', [
            (monday, 13, 14, 'Tiếng Anh Lớp 9 - Ôn thi vào lớp 10'),
            (monday + timedelta(days=2), 13, 14, 'Tiếng Anh Lớp 9 - Đề thi thử'),
            (monday + timedelta(days=7), 13, 14, 'Tiếng Anh Lớp 9 - Chữa bài tập'),
            (monday + timedelta(days=9), 13, 14, 'Tiếng Anh Lớp 9 - Hỏi đáp kiến thức'),
        ]),
        
        # Class 10 (GV Vũ Bích Ngọc)
        ('c_l10', 'r5', [
            (monday + timedelta(days=1), 1, 3, 'Tiếng Anh Lớp 10 - Cấu trúc câu phức'),
            (monday + timedelta(days=3), 1, 3, 'Tiếng Anh Lớp 10 - Bài đọc tiếng Anh'),
            (monday + timedelta(days=8), 1, 3, 'Tiếng Anh Lớp 10 - Viết câu phức'),
            (monday + timedelta(days=10), 1, 3, 'Tiếng Anh Lớp 10 - Ôn tập môn học'),
        ]),
        
        # Class 11 (GV Lê Hoàng Hải)
        ('c_l11', 'r6', [
            (monday + timedelta(days=2), 3, 5, 'Tiếng Anh Lớp 11 - Cấu trúc nâng cao'),
            (monday + timedelta(days=4), 3, 5, 'Tiếng Anh Lớp 11 - Bài mẫu văn xã hội'),
            (monday + timedelta(days=9), 3, 5, 'Tiếng Anh Lớp 11 - Kỹ năng viết lá thư'),
            (monday + timedelta(days=11), 3, 5, 'Tiếng Anh Lớp 11 - Chuẩn bị thi THPT'),
        ]),
        
        # Class 12 (GV Trịnh Văn Quyết)
        ('c_l12', 'r10', [
            (monday, 4, 6, 'Ôn thi THPT Quốc gia - Ngữ pháp'),
            (monday + timedelta(days=2), 4, 6, 'Ôn thi THPT Quốc gia - Từ vựng'),
            (monday + timedelta(days=7), 4, 6, 'Ôn thi THPT Quốc gia - Đề thi mẫu'),
            (monday + timedelta(days=9), 4, 6, 'Ôn thi THPT Quốc gia - Chữa đề thi'),
        ]),
        
        # IELTS 5.0 (GV Bùi Thu Hà)
        ('c_i1', 'r8', [
            (monday + timedelta(days=1), 12, 14, 'IELTS 5.0 - Listening practice'),
            (monday + timedelta(days=3), 12, 14, 'IELTS 5.0 - Reading comprehension'),
            (monday + timedelta(days=8), 12, 14, 'IELTS 5.0 - Writing skills'),
            (monday + timedelta(days=10), 12, 14, 'IELTS 5.0 - Speaking tips'),
        ]),
        
        # IELTS 6.0 (GV Hồ Quang Hiếu)
        ('c_i2', 'r9', [
            (monday + timedelta(days=2), 8, 10, 'IELTS 6.0 - Advanced listening'),
            (monday + timedelta(days=4), 8, 10, 'IELTS 6.0 - Academic reading'),
            (monday + timedelta(days=9), 8, 10, 'IELTS 6.0 - Essay writing'),
            (monday + timedelta(days=11), 8, 10, 'IELTS 6.0 - Fluency practice'),
        ]),
        
        # IELTS 7.0 (GV Nguyễn Lan Anh)
        ('c_i3', 'r10', [
            (monday, 10, 12, 'IELTS 7.0 - Native-like listening'),
            (monday + timedelta(days=2), 10, 12, 'IELTS 7.0 - Dense text reading'),
            (monday + timedelta(days=7), 10, 12, 'IELTS 7.0 - High-level writing'),
            (monday + timedelta(days=9), 10, 12, 'IELTS 7.0 - Sophisticated speaking'),
        ]),
    ]
    
    count = 0
    for class_key, room_key, sessions in schedules:
        class_id = classes[class_key]
        room_id = rooms[room_key]
        
        for date, start_slot, end_slot, title in sessions:
            cursor.execute(
                "INSERT INTO BuoiHoc (MaBuoiHoc, MaLopHoc, MaPhongHoc, NgayHoc, MaTietBatDau, MaTietKetThuc, TieuDe, DaXoa) VALUES (%s, %s, %s, %s, %s, %s, %s, %s)",
                (str(uuid.uuid4()), class_id, room_id, date, start_slot, end_slot, title, 0)
            )
            count += 1
    
    print(f"✓ Inserted {count} schedule sessions")

def verify_data(cursor):
    """Verify inserted data"""
    print("\n" + "="*50)
    print("📊 DATA VERIFICATION")
    print("="*50)
    
    checks = [
        ('TietHoc', 'SELECT COUNT(*) as count FROM TietHoc'),
        ('KhoaHoc', 'SELECT COUNT(*) as count FROM KhoaHoc'),
        ('LopHoc', 'SELECT COUNT(*) as count FROM LopHoc'),
        ('NguoiDung', 'SELECT COUNT(*) as count FROM NguoiDung'),
        ('GiangVien', 'SELECT COUNT(*) as count FROM GiangVien'),
        ('HocSinh', 'SELECT COUNT(*) as count FROM HocSinh'),
        ('PhuHuynh', 'SELECT COUNT(*) as count FROM PhuHuynh'),
        ('GiangVienLopHoc', 'SELECT COUNT(*) as count FROM GiangVienLopHoc'),
        ('HocSinhLopHoc', 'SELECT COUNT(*) as count FROM HocSinhLopHoc'),
        ('PhuHuynhHocSinh', 'SELECT COUNT(*) as count FROM PhuHuynhHocSinh'),
        ('PhongHoc', 'SELECT COUNT(*) as count FROM PhongHoc'),
        ('BuoiHoc', 'SELECT COUNT(*) as count FROM BuoiHoc'),
    ]
    
    for table_name, query in checks:
        cursor.execute(query)
        result = cursor.fetchone()
        count = result['count']
        print(f"✓ {table_name:25} {count:4} records")

def main():
    """Main seeding function"""
    print("\n" + "="*50)
    print("🌱 SEEDING DEMO DATA")
    print("="*50 + "\n")
    
    try:
        conn = get_connection()
        cursor = conn.cursor()
        
        clear_tables(cursor)
        seed_tiet_hoc(cursor)
        seed_khoa_hoc(cursor)
        seed_lop_hoc(cursor)
        seed_phong_hoc(cursor)
        seed_users_and_profiles(cursor)
        seed_relationships(cursor)
        seed_buoi_hoc(cursor)
        
        conn.commit()
        verify_data(cursor)
        
        cursor.close()
        conn.close()
        
        print("\n" + "="*50)
        print("✅ SEEDING COMPLETED SUCCESSFULLY!")
        print("="*50 + "\n")
        
    except Exception as e:
        print(f"\n❌ ERROR: {str(e)}")
        import traceback
        traceback.print_exc()

if __name__ == '__main__':
    main()
