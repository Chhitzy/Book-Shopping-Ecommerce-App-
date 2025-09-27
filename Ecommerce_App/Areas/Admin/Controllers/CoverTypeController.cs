using Dapper;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly ISP_CALL SP;

        public CoverTypeController(ISP_CALL SP_CALL)
        {
            SP= SP_CALL;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            CoverType covertype = new();
            if (id == null) return View(covertype);
            DynamicParameters parameters = new();
            parameters.Add("id", id.GetValueOrDefault());
            covertype = SP.OneRecord<CoverType>(SD.GetCoverType, parameters);
            if (covertype == null) return NotFound();
            return View(covertype);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType covertype)
        {
            if (covertype == null) return NotFound();
            if(!ModelState.IsValid)
            {
                return View(covertype);
            }
            DynamicParameters parameters = new();
            parameters.Add("name",covertype.Name);
            if (covertype.Id == 0)
            {
                SP.Execute(SD.CreateCoverType,parameters);
            }
            else
            {
                parameters.Add("id",covertype.Id);
                SP.Execute(SD.UpdateCoverType,parameters);
            }
            return RedirectToAction(nameof(Index));
        }


        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = SP.List<CoverType>(SD.GetCoverTypes) });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            DynamicParameters parameters = new();
            parameters.Add("id",id);
            var coverTypeInDb = SP.OneRecord<CoverType>(SD.GetCoverType,parameters);
            if(coverTypeInDb == null)
            {
                return Json(new { success = false, message = "Something Went Wrong While Deleting" });
            }
            else
            {
                SP.Execute(SD.DeleteCoverType,parameters);
                return Json(new { success = true, message = "CoverType Deleted Successfully" });
            }

        }

        #endregion
    }
}
