using Dapper;
using Ecommerce_App.Data;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ecommerce_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ISP_CALL SP;

        public CategoryController(ISP_CALL SP_CALL)
        {
            SP= SP_CALL;
        }
        public IActionResult Index() //For Main Page
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int ?id) { //For Adding Category Page, Can Use Update here as well

            Category category = new();
            if(id == null) return View(category);
            DynamicParameters parameter = new();
            parameter.Add("id", id.GetValueOrDefault());
            category = SP.OneRecord<Category>(SD.GetCategory, parameter);
            if(category == null) return NotFound();

            return View(category); 
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if(category == null) return NotFound();
            if(!ModelState.IsValid) return View(category);
            DynamicParameters parameters = new();
            parameters.Add("name", category.Name);
            if(category.Id == 0)
            {
                SP.Execute(SD.CreateCategory, parameters);
            }
            else
            {
                parameters.Add("id", category.Id);

                SP.Execute(SD.UpdateCategory, parameters);
            }
                return RedirectToAction(nameof(Index));
        }

        #region  APIs
        public IActionResult Delete(int id) // An Api used to delete Something during runtime , fast and efficient using javascript
        {
            DynamicParameters parameters = new();
            parameters.Add("id", id);
            var CovertypeInDb = SP.OneRecord<CoverType>(SD.DeleteCategory, parameters);
            if(CovertypeInDb == null)
            {
                return Json(new { success = false, message = "Something Went Wrong while Deleting " });
            }
            else
            {
                SP.Execute(SD.DeleteCategory, parameters);
                return Json(new { success = true, message = "Category Deleted Successfully" });
            }
        }

        public IActionResult GetAll()// An Api used to fetch all data, great for troubleshooting the project in the beginning
        {
            return Json(new { data = SP.List<Category>(SD.GetCategories) });
        }

        #endregion
    }
}
