using MySqlConnector;

const string connectionString = "server=127.0.0.1;port=3307;database=quanlytrungtam;user=root;password=root";

await using var connection = new MySqlConnection(connectionString);
await connection.OpenAsync();

async Task PrintQueryAsync(string title, string sql)
{
    Console.WriteLine($"=== {title} ===");
    await using var command = new MySqlCommand(sql, connection);
    await using var reader = await command.ExecuteReaderAsync();

    var columnCount = reader.FieldCount;
    while (await reader.ReadAsync())
    {
        for (var i = 0; i < columnCount; i++)
        {
            var name = reader.GetName(i);
            var value = await reader.IsDBNullAsync(i) ? "NULL" : reader.GetValue(i)?.ToString();
            Console.Write($"{name}={value}");
            if (i < columnCount - 1)
            {
                Console.Write(" | ");
            }
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}

await PrintQueryAsync(
    "NHOM DANH MUC",
    """
    SELECT MaNhom, MaCode, TenNhom
    FROM nhomdanhmuc
    ORDER BY MaNhom;
    """);

await PrintQueryAsync(
    "CHI TIET DANH MUC LIEN QUAN",
    """
    SELECT c.MaChiTiet, c.MaNhom, n.MaCode AS NhomCode, c.MaCode, c.TenChiTiet, c.ThuTu
    FROM chitietdanhmuc c
    JOIN nhomdanhmuc n ON n.MaNhom = c.MaNhom
    WHERE n.MaCode IN (
        'LOAI_BAI_TAP',
        'DANG_SU_KIEN',
        'TRANG_THAI_SU_KIEN',
        'LOAI_CAU_HOI',
        'MUC_DO_CAU_HOI',
        'TRANG_THAI_NOP_BAI'
    )
    ORDER BY c.MaNhom, c.ThuTu, c.MaChiTiet;
    """);

await PrintQueryAsync(
    "BAI TAP MAU",
    """
    SELECT b.MaBaiTap, b.TenBaiTap, b.LoaiBaiTap, lbt.MaCode AS LoaiCode, lbt.TenChiTiet AS LoaiTen,
           b.MaKhoaHoc, kh.TenKhoaHoc, b.ThoiGianLamBai, b.DiemToiDa, b.TrangThai, b.DaXoa
    FROM baitap b
    LEFT JOIN chitietdanhmuc lbt ON lbt.MaChiTiet = b.LoaiBaiTap
    JOIN khoahoc kh ON kh.MaKhoaHoc = b.MaKhoaHoc
    ORDER BY b.ThoiGianTao DESC
    LIMIT 20;
    """);

await PrintQueryAsync(
    "SU KIEN LOP HOC GAN BAI TAP",
    """
    SELECT sk.MaSuKien, sk.MaBaiTap, b.TenBaiTap, sk.MaLopHoc, lh.TenLop,
           sk.DangSuKien, dsk.MaCode AS DangSuKienCode, dsk.TenChiTiet AS DangSuKienTen,
           sk.MaTrangThai, ttsk.MaCode AS TrangThaiCode, ttsk.TenChiTiet AS TrangThaiTen,
           sk.HanNop, sk.TrangThai, sk.DaXoa
    FROM sukienlophoc sk
    JOIN baitap b ON b.MaBaiTap = sk.MaBaiTap
    JOIN lophoc lh ON lh.MaLopHoc = sk.MaLopHoc
    LEFT JOIN chitietdanhmuc dsk ON dsk.MaChiTiet = sk.DangSuKien
    LEFT JOIN chitietdanhmuc ttsk ON ttsk.MaChiTiet = sk.MaTrangThai
    ORDER BY sk.ThoiGianTao DESC
    LIMIT 20;
    """);

await PrintQueryAsync(
    "CAU HOI MAU",
    """
    SELECT q.MaCauHoi, q.MaKhoaHoc, kh.TenKhoaHoc, q.LoaiCauHoi, lch.MaCode AS LoaiCode, lch.TenChiTiet AS LoaiTen,
           q.MucDo, md.MaCode AS MucDoCode, md.TenChiTiet AS MucDoTen, q.MucDichSuDung,
           LEFT(q.NoiDungCauHoi, 120) AS NoiDung
    FROM nganhangcauhoi q
    JOIN khoahoc kh ON kh.MaKhoaHoc = q.MaKhoaHoc
    LEFT JOIN chitietdanhmuc lch ON lch.MaChiTiet = q.LoaiCauHoi
    LEFT JOIN chitietdanhmuc md ON md.MaChiTiet = q.MucDo
    ORDER BY q.ThoiGianTao DESC
    LIMIT 20;
    """);

await PrintQueryAsync(
    "DAP AN MAU",
    """
    SELECT d.MaDapAn, d.MaCauHoi, d.NoiDungDapAn, d.LaDapAnDung
    FROM dapan d
    ORDER BY d.ThoiGianTao DESC
    LIMIT 20;
    """);
