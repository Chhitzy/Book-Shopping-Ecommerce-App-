using Dapper;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Ecommerce_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ISP_CALL SP;
        public CompanyController(ISP_CALL sP_CALL)
        {
            SP = sP_CALL;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null) return View(company);
            DynamicParameters parameters = new();
            parameters.Add("id", id.GetValueOrDefault());
            company = SP.OneRecord<Company>(SD.GetCompany, parameters);
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Company company)
        {
            if (company == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(company);
            }
            DynamicParameters parameters = new();
            
            if (company.Id == 0)
            {
                parameters.Add("name", company.Name);
                parameters.Add("city", company.City);
                parameters.Add("state", company.State);
                parameters.Add("streetaddress", company.StreetAddress);
                parameters.Add("phonenumber", company.PhoneNumber);
                parameters.Add("isauthorizedcompany", company.IsAuthorizedCompany);
                parameters.Add("postalcode", company.PostalCode);
                SP.Execute(SD.CreateCompany, parameters);
            }
            else
            {
                parameters.Add("id", company.Id);
                parameters.Add("name", company.Name);
                parameters.Add("city", company.City);
                parameters.Add("state", company.State);
                parameters.Add("streetaddress", company.StreetAddress);
                parameters.Add("phonenumber", company.PhoneNumber);
                parameters.Add("isauthorizedcompany", company.IsAuthorizedCompany);
                parameters.Add("postalcode", company.PostalCode);
                SP.Execute(SD.UpdateCompany, parameters);
            }
            return RedirectToAction(nameof(Index));
        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
           
            return Json(new { data = SP.List<Company>(SD.GetCompanies) });
        }

        [HttpGet]

        public IActionResult Delete(int id)
        {
            DynamicParameters param = new();
            param.Add("id", id);
            var companyid = SP.OneRecord<Company>(SD.GetCompany,param);
            if(companyid == null)
            {
                return Json(new {success = false,message = "Something Went Wrong While Deleting"});
            }
            else
            {
                SP.Execute(SD.DeleteCompany,param);
                return Json(new { success = true, message = "Company Deleted Successfully" });
            }

                
        }



        #endregion


    }
}
