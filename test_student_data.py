import pymysql

conn = pymysql.connect(
    host='127.0.0.1',
    port=3306,
    user='root',
    password='',
    database='HeThongHocOnline_Final',
    charset='utf8mb4'
)

cursor = conn.cursor(pymysql.cursors.DictCursor)

# Check if there are any students
cursor.execute("SELECT COUNT(*) as count FROM HocSinh")
result = cursor.fetchone()
print(f"Total Students: {result['count']}")

# Check student-class relationships
cursor.execute("SELECT COUNT(*) as count FROM HocSinhLopHoc")
result = cursor.fetchone()
print(f"Total Student-Class enrollments: {result['count']}")

# Check specific student data
cursor.execute("""
SELECT h.MaHocSinh, nd.HoTen, COUNT(hsl.MaLopHoc) as classCount
FROM HocSinh h
JOIN NguoiDung nd ON h.MaNguoiDung = nd.MaNguoiDung  
LEFT JOIN HocSinhLopHoc hsl ON h.MaHocSinh = hsl.MaHocSinh
GROUP BY h.MaHocSinh
LIMIT 5
""")
for row in cursor.fetchall():
    print(f"Student: {row['HoTen']} (ID: {row['MaHocSinh']}) -> {row['classCount']} classes")

cursor.close()
conn.close()
