using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;

namespace SoftVersionControl.Controllers
{
    public class HistoryController : Controller
    {
        private readonly AppDbContext _context;
        public HistoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lichsu()
        {
            ViewBag.parentPage = "Trang chủ";
            ViewBag.currentPage = "Lịch sử hoạt động";
            var list = await _context.Lichsus.OrderBy(x => x.Thoigian).ToListAsync();
            return View(list);
        }

        public IActionResult CreateChart(string mode)
        {
            DateTime now = DateTime.Now;
            var modeXem = DateTime.Now.Date.AddHours(-10);
            if (mode == "ngay")
            {
            }
            if (mode == "tuan")
            {
                // Tính toán ngày đầu tiên của tuần (bắt đầu từ Chủ Nhật)
                DateTime firstDayOfWeek = now.Date; // Đặt ngày giờ thành 00:00:00 để bắt đầu từ đầu ngày
                while (firstDayOfWeek.DayOfWeek != DayOfWeek.Sunday)
                {
                    firstDayOfWeek = firstDayOfWeek.AddDays(-1);
                }
                // Tính toán ngày cuối cùng của tuần (kết thúc vào Chủ Nhật)
                DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6); // Tuần có 7 ngày, nên cộng thêm 6 ngày để lấy ngày cuối cùng
                TimeSpan thisWeek = lastDayOfWeek - firstDayOfWeek;
                int numberOfDays = thisWeek.Days;

                modeXem = DateTime.Now.AddDays(-numberOfDays);
            }
            if (mode == "thang")
            {
                // Lấy ngày đầu tiên của tháng hiện tại
                DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                // Lấy ngày cuối cùng của tháng hiện tại bằng cách lấy ngày đầu tiên của tháng sau đó trừ đi 1 ngày
                DateTime lastDayOfMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
                // Tính toán khoảng thời gian cho tháng này
                TimeSpan thisMonth = lastDayOfMonth - firstDayOfMonth;
                // Lấy số ngày trong khoảng thời gian của tháng này
                int numberOfDaysInMonth = thisMonth.Days;

                modeXem = DateTime.Now.AddDays(-numberOfDaysInMonth);
            }

            var typeToRemove = "Login";
            var typeToRemove2 = "Upload";

            var typeCounts = _context.Lichsus
                .Where(l => l.Thoigian >= modeXem) 
                .GroupBy(l => l.Type)
                .Select(g => new
                {
                    TypeName = g.Key,
                    Count = g.Count()
                })
                 .Where(tc => (tc.TypeName != typeToRemove && tc.TypeName != typeToRemove2))
                .ToList();
            // Tạo các danh sách chứa dữ liệu cho biểu đồ
            var typeNames = typeCounts.Select(tc => tc.TypeName).ToList();
            var typeCountsList = typeCounts.Select(tc => tc.Count).ToList();
            ViewBag.mode = mode;
            return Json(new { success = true, name = typeNames, count = typeCountsList });
        }

    }
}
