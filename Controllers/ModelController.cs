using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;

namespace SoftVersionControl.Controllers
{
    public class ModelController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public ModelController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> LuuLichSu(string type, string hanhdong)
        {
            ISession session = HttpContext.Session;
            string role = session.GetString("userrole").ToString();
            Lichsu lichsu = new Lichsu();
            if (role == "admin")
            {
                lichsu.Ten = session.GetString("username").ToString() + " " + "(Admin)";
            }
            else
            {
                lichsu.Ten = session.GetString("username").ToString();
            }
            lichsu.Hanhdong = hanhdong;
            lichsu.Thoigian = DateTime.Now;
            lichsu.Type = type;
            _context.Lichsus.Add(lichsu);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetSelectListParentModels()
        {
            List<Model> parentModels = await _context.Models.Where(x => x.ParentModelId == null).ToListAsync();
            return Json(parentModels);
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectListChildrenModels()
        {
            List<Model> childrenModels = await _context.Models.Where(x => x.ParentModelId != null).ToListAsync();
            return Json(childrenModels);
        }

        [HttpGet]
        public async Task<IActionResult> GetModels()
        {
            var getAllModel = (from c in _context.Models select c)
               .Include(c => c.ParentModel)
               .Include(c => c.ChildrenModel)
               .Include(s => s.SoftNames);
            var parentModels = (await getAllModel.ToListAsync())
                .Where(c => c.ParentModelId == null)
                .ToList();
            return PartialView("_ModelList", parentModels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel(string name, int modelTong)
        {
            var checktrung = _context.Models.FirstOrDefault(x => x.Name == name.ToUpper());
            if (checktrung != null)
            {
                return Json(new { success = false, message = "Model đã tồn tại!" });
            }
            if (modelTong == -1)
            {
                var newParentModel = new Model { Name = name.ToUpper(), ParentModelId = null };
                _context.Add(newParentModel);
                await _context.SaveChangesAsync();
                await LuuLichSu("Create", "Tạo Model: " + name.ToUpper());
                return Json(new { success = true, data = newParentModel });
            }
            else
            {
                var newChildrenModel = new Model { Name = name.ToUpper(), ParentModelId = modelTong };
                _context.Add(newChildrenModel);
                await _context.SaveChangesAsync();
                await LuuLichSu("Create", "Tạo Model: " + name.ToUpper());
                return Json(new { success = true, data = newChildrenModel });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetModelNameFromId(int id)
        {
            var model = await _context.Models.Where(x => x.Id == id).FirstOrDefaultAsync();
            string nameModel = model.Name.ToString();
            return Json(new { success = true, data = nameModel });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteModel(int id)
        {
            try
            {
                var model = await _context.Models
                            .Include(c => c.ChildrenModel)
                            .FirstOrDefaultAsync(c => c.Id == id);
                if (model != null)
                {
                    // Xoá tất cả model con
                    foreach (var childModel in model.ChildrenModel)
                    {
                        _context.Models.Remove(childModel);
                    }
                    // Xoá model cha
                    _context.Models.Remove(model);
                    await _context.SaveChangesAsync();
                    await LuuLichSu("Delete", "Xoá Model: " + model.Name);
                    return Json(new { success = true, message = "Xoá thành công." });
                }
                return Json(new { success = false, message = "Không tìm thấy dữ liệu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> checkToChangeNameModel(int id)
        {
            var check = await _context.SoftNames.Where(x => x.ModelId == id).CountAsync();
            if (check > 0)
            {
                return Json(new { success = false, mess = "Đã có dữ liệu" });
            }
            return Json(new { success = true, mess = "Có thể đổi tên" });
        }

        [HttpPost]
        public async Task<IActionResult> changeNameModel(int id, string tenMoi)
        {
            var model = await _context.Models.Where(x => x.Id == id).FirstOrDefaultAsync();
            string tencu = model.Name;
            if (model != null)
            {
                model.Name = tenMoi;
                await LuuLichSu("Edit", "Sửa tên Model " + tencu + " thành: " +tenMoi);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }



    }
}
