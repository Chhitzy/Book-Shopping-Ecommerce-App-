using Dapper;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderHistory : Controller
    {
        private readonly ISP_CALL SP;
        public OrderHistory(ISP_CALL sP_CALL)
        {
            SP = sP_CALL;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs

        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            DynamicParameters param = new();
            if (claim == null) NotFound();
            param.Add("@id", claim.Value);
            var all = SP.List<OrderHeader>(SD.GetAllOrderHeaderDetails, param);
            return Json(new { all });
        }
        #endregion

        public IActionResult NavigateToHelpAndSupport()
        {
            return View();
        }
    }
}
