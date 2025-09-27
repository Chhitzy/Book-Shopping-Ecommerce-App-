using Dapper;
using Ecommerce_App.Data;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecommerce_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ISP_CALL SP;
        private readonly ApplicationDbContext _context;
        public UserController(ISP_CALL sP_CALL, ApplicationDbContext context)
        {
            SP = sP_CALL;
            _context = context;
        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var UserList = _context.ApplicationUsers.ToList();
            var RoleList = _context.Roles.ToList();
            var UserRoles = _context.UserRoles.ToList();

            foreach (var user in UserList)
            {


                var RoleId = UserRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId;

                user.Role = RoleList.FirstOrDefault(r => r.Id == RoleId).Name;
                DynamicParameters param = new();

                if (user.CompanyId == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
                if (user.CompanyId != null)
                {
                    param.Add("id", Convert.ToInt32(user.CompanyId));
                    var company = SP.OneRecord<Company>(SD.GetCompany, param);
                    user.Name = company?.Name;
                }

            }
            var adminUser = UserList.FirstOrDefault(u => u.Role == SD.Role_Admin);

            if (adminUser != null)
            {
                UserList.Remove(adminUser);
            }
            return Json(new { data = UserList });

        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            bool isLocked = false;
            var userIndb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userIndb == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong while lock and unlock user !!!"
                });

            }
            if (userIndb != null && userIndb.LockoutEnd > DateTime.Now)
            {
                userIndb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                userIndb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new
            {
                success = true,
                message = isLocked == true ?
                "User Successfully locked" : "User successfully unlocked"
            });
        }

        #endregion


        public IActionResult Index()
        {
            return View();
        }
    }
}
