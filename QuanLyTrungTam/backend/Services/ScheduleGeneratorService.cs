using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class ScheduleGeneratorService
    {
        private readonly AppDbContext _db;

        public ScheduleGeneratorService(AppDbContext db)
        {
            _db = db;
        }

        private static int MapDayNameToDayOfWeek(string name)
        {
            return name switch
            {
                "Thu 2" => 1,
                "Thu 3" => 2,
                "Thu 4" => 3,
                "Thu 5" => 4,
                "Thu 6" => 5,
                "Thu 7" => 6,
                "Chu nhat" => 0,
                _ => throw new ArgumentException("Invalid day name: " + name)
            };
        }

        public async Task<(bool Success, string? Error, DateOnly? EndDate)> GenerateAndPersistAsync(Guid classId, UpsertClassRequestDto request)
        {
            // If TotalPeriods not provided, do not treat as error — generation will be skipped.
            var totalNeeded = request.TotalPeriods.HasValue && request.TotalPeriods.Value > 0
                ? request.TotalPeriods.Value
                : 0;
            if (totalNeeded == 0)
            {
                // No total provided -> skip generation and return success with no end date.
                return (true, null, null);
            }
            var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

            // Build weekly patterns
            var patterns = new List<(int Weekday, int Start, int End, Guid? Room)>();
            foreach (var cfg in request.ScheduleConfigs ?? Enumerable.Empty<ScheduleConfigDto>())
            {
                if (cfg.Periods == null || cfg.Periods.Count == 0) continue;
                var start = cfg.Periods.Min();
                var end = cfg.Periods.Max();
                foreach (var d in cfg.Days ?? Enumerable.Empty<string>())
                {
                    var wd = MapDayNameToDayOfWeek(d);
                    patterns.Add((wd, start, end, cfg.RoomId));
                }
            }

            if (patterns.Count == 0)
            {
                return (false, "Khong co cau hinh lich (chua chon ngay/tiet).", null);
            }

            try
            {
                var created = new List<Buoihoc>();
                int createdPeriods = 0;

                // We'll iterate week by week until we reach the required number of periods
                // Logic: for each week, calculate what date each pattern should be scheduled
                for (int weekOffset = 0; createdPeriods < totalNeeded && weekOffset < 520; weekOffset++) // safety: max 10 years
                {
                    var weekDates = new List<(DateOnly Date, int Start, int End, Guid? Room)>();
                    
                    foreach (var p in patterns)
                    {
                        // p.Weekday is 0-6 (Sunday=0, Monday=1, ..., Saturday=6)
                        // startDate.DayOfWeek is same format in .NET
                        // Find the date for this pattern in the current week offset
                        
                        var currentWeekStartDate = startDate.AddDays(weekOffset * 7);
                        var currentWeekDayOfWeek = (int)currentWeekStartDate.DayOfWeek;
                        
                        // How many days from current week start to reach the target weekday?
                        var daysFromWeekStart = (p.Weekday - currentWeekDayOfWeek + 7) % 7;
                        
                        // If daysFromWeekStart == 0, it means the pattern's weekday matches the week start
                        // We want to start from the week start itself
                        var date = currentWeekStartDate.AddDays(daysFromWeekStart);
                        weekDates.Add((date, p.Start, p.End, p.Room));
                    }

                    // sort by date within the week
                    weekDates = weekDates.OrderBy(x => x.Date.ToDateTime(TimeOnly.MinValue)).ToList();

                    foreach (var entry in weekDates)
                    {
                        if (createdPeriods >= totalNeeded) break;

                        var patternLength = entry.End - entry.Start + 1;
                        var remainingPeriods = totalNeeded - createdPeriods;
                        var sessionLength = Math.Min(patternLength, remainingPeriods);
                        var scheduledEnd = entry.Start + sessionLength - 1;

                        // conflict checks
                        var existsClassConflict = await _db.Buoihocs.AnyAsync(b =>
                            b.MaLopHoc == classId &&
                            b.NgayHoc == entry.Date &&
                            b.MaTietBatDau <= scheduledEnd &&
                            b.MaTietKetThuc >= entry.Start &&
                            (b.DaXoa == null || !b.DaXoa.Value));
                        if (existsClassConflict)
                        {
                            return (false, $"Trung lich voi chinh lop hoc tren ngay {entry.Date} tiet {entry.Start}-{scheduledEnd}.", null);
                        }

                        // teacher conflict: any existing BuoiHoc on same date/time where any of the requested teachers are teaching
                        if (request.TeacherIds != null && request.TeacherIds.Count > 0)
                        {
                            var conflictTeacher = await _db.Buoihocs
                                .Where(b => b.NgayHoc == entry.Date && b.MaTietBatDau <= scheduledEnd && b.MaTietKetThuc >= entry.Start && (b.DaXoa == null || !b.DaXoa.Value))
                                .Join(_db.Giangvienlophocs.Where(g => request.TeacherIds.Contains(g.MaGiangVien) && (g.DaXoa == null || !g.DaXoa.Value)),
                                    b => b.MaLopHoc,
                                    g => g.MaLopHoc,
                                    (b, g) => b)
                                .FirstOrDefaultAsync();

                            if (conflictTeacher != null)
                            {
                                return (false, $"Trung lich giang vien tren ngay {entry.Date} tiet {entry.Start}-{scheduledEnd}.", null);
                            }
                        }

                        // room conflict
                        if (entry.Room.HasValue)
                        {
                            var conflictRoom = await _db.Buoihocs.FirstOrDefaultAsync(b =>
                                b.NgayHoc == entry.Date &&
                                b.MaTietBatDau <= scheduledEnd &&
                                b.MaTietKetThuc >= entry.Start &&
                                b.MaPhongHoc == entry.Room &&
                                (b.DaXoa == null || !b.DaXoa.Value));
                            if (conflictRoom != null)
                            {
                                return (false, $"Phong hoc da duoc su dung tren ngay {entry.Date} tiet {entry.Start}-{scheduledEnd}.", null);
                            }
                        }

                        // create buoi hoc
                        var bh = new Buoihoc
                        {
                            MaBuoiHoc = Guid.NewGuid(),
                            MaLopHoc = classId,
                            MaPhongHoc = entry.Room,
                            NgayHoc = entry.Date,
                            MaTietBatDau = entry.Start,
                            MaTietKetThuc = scheduledEnd,
                            TieuDe = request.Name,
                            NoiDung = $"Lich hoc {entry.Date} tiet {entry.Start}-{scheduledEnd}",
                            ThoiGianTao = DateTime.UtcNow,
                            TrangThai = true,
                            DaXoa = false
                        };

                        _db.Buoihocs.Add(bh);
                        created.Add(bh);
                        createdPeriods += sessionLength;
                    }
                }

                if (created.Count == 0)
                {
                    return (false, "Khong co buoi hoc duoc tao.", null);
                }

                // Do NOT call SaveChangesAsync here — let the controller manage transaction and persistence
                // Just return the last date for the caller to use
                var lastDate = created.Max(x => x.NgayHoc);
                return (true, null, lastDate);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
