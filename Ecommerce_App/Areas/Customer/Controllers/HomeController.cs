using Dapper;
using Ecommerce_App.Data;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Models.ViewModels;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecommerce_App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISP_CALL SP;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ISP_CALL sP_CALL,ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            SP = sP_CALL;
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claims != null)
            {
                var count = _context.ShoppingCarts.Where(s => s.ApplicationUserId == claims.Value).Count();
                HttpContext.Session.SetInt32(SD.SS_ShoppingCartCountSession, count);
            }

            var ProductList = SP.List<Product>(SD.GetProducts);
            return View(ProductList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details (int id)
        {
            DynamicParameters param = new();
            param.Add("id", id);
            
            var ProductInDb = SP.OneRecord<Product>(SD.GetProduct, param);

           


            if (ProductInDb == null) return NotFound();

            DynamicParameters paramforcat = new();
            paramforcat.Add("id",ProductInDb.CategoryId);
            var category = SP.OneRecord<Category>(SD.GetCategory,paramforcat);

            DynamicParameters paramforcovertype = new();
            paramforcovertype.Add("id", ProductInDb.CoverTypeId);
            var covertype = SP.OneRecord<CoverType>(SD.GetCoverType,paramforcovertype);

            ProductInDb.Category = category;
            ProductInDb.CoverType = covertype;

            var shoppingCart = new ShoppingCart()
            {
                Product = ProductInDb,
                ProductId = ProductInDb.Id,
                Count =1 
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]

        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)(User.Identity);
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claims == null) return NotFound();
                shoppingCart.ApplicationUserId = claims.Value;

                DynamicParameters param = new();
                param.Add("@ApplicationUserId", shoppingCart.ApplicationUserId);
                param.Add("@ProductId", shoppingCart.ProductId);
                param.Add("@Count", shoppingCart.Count);

                SP.Execute(SD.AddOrUpdateShoppingCart, param);

                return RedirectToAction("Index");

            }
            else
            {
                DynamicParameters param = new();
                param.Add("id", shoppingCart.ProductId);

                var ProductInDb = SP.OneRecord<Product>(SD.GetProduct, param);




                if (ProductInDb == null) return NotFound();

                DynamicParameters paramforcat = new();
                paramforcat.Add("id", ProductInDb.CategoryId);
                var category = SP.OneRecord<Category>(SD.GetCategory, paramforcat);

                DynamicParameters paramforcovertype = new();
                paramforcovertype.Add("id", ProductInDb.CoverTypeId);
                var covertype = SP.OneRecord<CoverType>(SD.GetCoverType, paramforcovertype);

                ProductInDb.Category = category;
                ProductInDb.CoverType = covertype;


                shoppingCart.Product = ProductInDb;
                shoppingCart.ProductId = ProductInDb.Id;
                shoppingCart.Count = 1;

                return View(shoppingCart);
            }
        }

    }
}
