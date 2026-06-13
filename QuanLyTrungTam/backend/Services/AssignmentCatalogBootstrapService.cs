using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class AssignmentCatalogBootstrapService
{
    private readonly AppDbContext _context;

    public AssignmentCatalogBootstrapService(AppDbContext context)
    {
        _context = context;
    }

    public async Task EnsureInitializedAsync()
    {
        var groups = new[]
        {
            new
            {
                Code = "LOAI_CAU_HOI",
                Name = "Loai cau hoi",
                Details = new[]
                {
                    new SeedDetail("LCH_MCQ", "Trac nghiem", 1),
                    new SeedDetail("LCH_MATCHING", "Noi ghep", 2),
                    new SeedDetail("LCH_GAPFILL", "Dien khuyet", 3),
                    new SeedDetail("LCH_TRANSFORM", "Viet lai cau", 4),
                }
            },
            new
            {
                Code = "MUC_DO_CAU_HOI",
                Name = "Muc do cau hoi",
                Details = new[]
                {
                    new SeedDetail("DE", "De", 1),
                    new SeedDetail("TRUNG_BINH", "Trung binh", 2),
                    new SeedDetail("KHO", "Kho", 3),
                }
            },
            new
            {
                Code = "LOAI_BAI_TAP",
                Name = "Loai bai tap",
                Details = new[]
                {
                    new SeedDetail("HOMEWORK", "Bai tap", 1),
                    new SeedDetail("QUIZ", "Kiem tra nhanh", 2),
                    new SeedDetail("EXAM", "De thi", 3),
                }
            },
            new
            {
                Code = "DANG_SU_KIEN",
                Name = "Dang su kien lop hoc",
                Details = new[]
                {
                    new SeedDetail("GIAO_BAI", "Giao bai tap", 1),
                    new SeedDetail("TO_CHUC_KIEM_TRA", "To chuc kiem tra", 2),
                    new SeedDetail("TO_CHUC_THI", "To chuc thi", 3),
                }
            },
            new
            {
                Code = "TRANG_THAI_SU_KIEN",
                Name = "Trang thai su kien",
                Details = new[]
                {
                    new SeedDetail("DRAFT", "Nhap", 1),
                    new SeedDetail("SCHEDULED", "Da len lich", 2),
                    new SeedDetail("ACTIVE", "Dang mo", 3),
                    new SeedDetail("CLOSED", "Da dong", 4),
                }
            },
            new
            {
                Code = "TRANG_THAI_NOP_BAI",
                Name = "Trang thai nop bai",
                Details = new[]
                {
                    new SeedDetail("NOT_STARTED", "Chua bat dau", 1),
                    new SeedDetail("IN_PROGRESS", "Dang lam bai", 2),
                    new SeedDetail("SUBMITTED", "Da nop bai", 3),
                    new SeedDetail("GRADED", "Da cham diem", 4),
                    new SeedDetail("LATE", "Nop tre", 5),
                }
            }
        };

        foreach (var groupSeed in groups)
        {
            var group = await _context.Nhomdanhmucs
                .FirstOrDefaultAsync(x => x.MaCode == groupSeed.Code);

            if (group == null)
            {
                group = new Nhomdanhmuc
                {
                    MaCode = groupSeed.Code,
                    TenNhom = groupSeed.Name,
                    TrangThai = true,
                    DaXoa = false
                };
                _context.Nhomdanhmucs.Add(group);
                await _context.SaveChangesAsync();
            }
            else if (group.DaXoa == true || group.TrangThai == false || group.TenNhom != groupSeed.Name)
            {
                group.TenNhom = groupSeed.Name;
                group.DaXoa = false;
                group.TrangThai = true;
                await _context.SaveChangesAsync();
            }

            foreach (var detailSeed in groupSeed.Details)
            {
                var detail = await _context.Chitietdanhmucs
                    .FirstOrDefaultAsync(x => x.MaNhom == group.MaNhom && x.MaCode == detailSeed.Code);

                if (detail == null)
                {
                    _context.Chitietdanhmucs.Add(new Chitietdanhmuc
                    {
                        MaNhom = group.MaNhom,
                        MaCode = detailSeed.Code,
                        TenChiTiet = detailSeed.Name,
                        ThuTu = detailSeed.Order,
                        TrangThai = true,
                        DaXoa = false
                    });
                }
                else
                {
                    detail.TenChiTiet = detailSeed.Name;
                    detail.ThuTu = detailSeed.Order;
                    detail.TrangThai = true;
                    detail.DaXoa = false;
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    private sealed record SeedDetail(string Code, string Name, int Order);
}
