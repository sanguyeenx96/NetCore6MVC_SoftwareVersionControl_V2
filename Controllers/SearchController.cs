using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;

namespace SoftVersionControl.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly AppDbContext _context;
        public SearchController(ILogger<SearchController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.parentPage = "Trang chủ";
            ViewBag.currentPage = "Tìm kiếm";

            var tenModel = await _context.Models.Select(x => x.Name).Distinct().ToListAsync();
            var selectlist = new SelectList(tenModel, tenModel);
            ViewBag.sl = selectlist;
            return View();
        }




        [HttpGet]
        public async Task<IActionResult> SearchSoftware(string searchTerm, string models, string trangThai)
        {
            try
            {
                var modelArray = models.Split(',');
                var trangThaiBool = trangThai == "Đang áp dụng" ? true : trangThai == "Không áp dụng" ? false : (bool?)null;

                //1-1
                if (modelArray.Contains("Tất cả") && trangThaiBool == null)
                {
                    var results = await _context.Softwares.Include(x => x.SoftName).Include(x=>x.SoftName.Model)
                        .Where(s => (string.IsNullOrEmpty(searchTerm) || s.SoftName.Name.Contains(searchTerm)))
                        .ToListAsync();
                    return PartialView("_ketquatimkiem", results);
                }
                //1-0
                if (modelArray.Contains("Tất cả") && trangThaiBool != null)
                {
                    var results = await _context.Softwares.Include(x => x.SoftName).Include(x => x.SoftName.Model)
                            .Where(s => (string.IsNullOrEmpty(searchTerm) || s.SoftName.Name.Contains(searchTerm)) &&
                                    s.TrangThaiApDung == trangThaiBool)
                            .ToListAsync();
                    return PartialView("_ketquatimkiem", results);
                }
                              //0-1
                if (!modelArray.Contains("Tất cả") && trangThaiBool == null)
                {
                    var results = await _context.Softwares.Include(x => x.SoftName).Include(x => x.SoftName.Model)
                            .Where(s => (string.IsNullOrEmpty(searchTerm) || s.SoftName.Name.Contains(searchTerm)) &&
                                     (modelArray.Length == 0 || modelArray.Contains(s.SoftName.Model.Name)))
                            .ToListAsync();
                    return PartialView("_ketquatimkiem", results);
                }
                //else if (!modelArray.Contains("Tất cả") && trangThaiBool != null)
                else
                {
                    var results = await _context.Softwares.Include(x => x.SoftName).Include(x => x.SoftName.Model)
                            .Where(s => (string.IsNullOrEmpty(searchTerm) || s.SoftName.Name.Contains(searchTerm)) &&
                                     (modelArray.Length == 0 || modelArray.Contains(s.SoftName.Model.Name)) &&
                                    (s.TrangThaiApDung == trangThaiBool))
                            .ToListAsync();
                    return PartialView("_ketquatimkiem", results);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
    }
}
