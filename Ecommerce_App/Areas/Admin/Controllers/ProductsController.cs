using Dapper;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Models.ViewModels;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly ISP_CALL SP;
        private readonly IWebHostEnvironment env;
        public ProductsController(ISP_CALL SP_CALL, IWebHostEnvironment webHostEnvironment)
        {
            SP = SP_CALL;
            env = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int ?id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),

                CategoryList = SP.List<Category>(SD.GetCategories).Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                CoverTypeList = SP.List<CoverType>(SD.GetCoverTypes).Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                    
                })

            };
            if (id == null) return View(productVM);
            DynamicParameters parameters = new();
            parameters.Add("id",id.GetValueOrDefault());
            productVM.Product = SP.OneRecord<Product>(SD.GetProduct,parameters);
            if(productVM.Product == null) return NotFound();
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            DynamicParameters parameters = new DynamicParameters();
            if (productVM.Product == null) return NotFound();
            if (ModelState.IsValid)
            {
                var webRootPath = env.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var uploads = Path.Combine(webRootPath, @"images/Products");

                    if (productVM.Product.Id != 0)
                    {
                        parameters.Add("id", productVM.Product.Id);
                        var ImageExists = SP.OneRecord<Product>(SD.GetProduct,parameters)?.ImageUrl;
                        productVM.Product.ImageUrl = ImageExists;
                    }
                    if (productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension ), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Products\"+ fileName +extension;
                }
                else
                {
                    if (productVM.Product.Id != 0)
                    {
                        parameters.Add("id", productVM.Product.Id);
                        var ImageExists = SP.OneRecord<Product>(SD.GetProduct,parameters)?.ImageUrl;
                        productVM.Product.ImageUrl = ImageExists;
                    }
                }


                if (productVM.Product.Id == 0)
                { 
                    parameters.Add("Title", productVM.Product.Title);
                    parameters.Add("Description", productVM.Product.Description);
                    parameters.Add("Author", productVM.Product.Author);
                    parameters.Add("ISBN", productVM.Product.ISBN);
                    parameters.Add("CategoryId", productVM.Product.CategoryId);
                    parameters.Add("CoverTypeId", productVM.Product.CoverTypeId);
                    parameters.Add("ListedPrice", productVM.Product.ListedPrice);
                    parameters.Add("Price", productVM.Product.Price);
                    parameters.Add("Price50", productVM.Product.Price50);
                    parameters.Add("Price100", productVM.Product.Price100);
                    parameters.Add("ImageUrl", productVM.Product.ImageUrl);
                    SP.Execute(SD.CreateProduct, parameters);
                }
                else
                {

                    parameters.Add("id", productVM.Product.Id);
                    parameters.Add("Title", productVM.Product.Title);
                    parameters.Add("Description", productVM.Product.Description);
                    parameters.Add("Author", productVM.Product.Author);
                    parameters.Add("ISBN", productVM.Product.ISBN);
                    parameters.Add("CategoryId", productVM.Product.CategoryId);
                    parameters.Add("CoverTypeId", productVM.Product.CoverTypeId);
                    parameters.Add("ListedPrice", productVM.Product.ListedPrice);
                    parameters.Add("Price", productVM.Product.Price);
                    parameters.Add("Price50", productVM.Product.Price50);
                    parameters.Add("Price100", productVM.Product.Price100);
                    parameters.Add("ImageUrl", productVM.Product.ImageUrl);
                    SP.Execute(SD.UpdateProduct, parameters);
                }

                return RedirectToAction("Index");
            }
            else
            {
                productVM = new ProductVM()
                {
                    Product = new Product(),
                    CategoryList = SP.List<Category>(SD.GetCategories).Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.Id.ToString()
                    }),
                    CoverTypeList = SP.List<CoverType>(SD.GetCoverTypes).Select(b => new SelectListItem
                    {
                        Text = b.Name,
                        Value = b.Id.ToString()
                    })
                };
                if(productVM.Product.Id != 0)
                {
                    DynamicParameters param = new();
                    param.Add("id", productVM.Product.Id);
                    productVM.Product = SP.OneRecord<Product>(SD.GetProduct, param);
                }
                return View(productVM);
            }
           
        }

        #region APIs

        public IActionResult GetAll()
        {
            return Json(new { data = SP.List<Product>(SD.GetProducts)}); 
        }
        [HttpDelete]

        public IActionResult Delete(int id)
        {
            DynamicParameters parameters = new();
            parameters.Add("id", id);
            var Product = SP.OneRecord<Product>(SD.GetProduct, parameters);
            if(Product == null)
            {
                return Json(new {success = false, message ="Error, Deletion Not Successful!" });
            }
            else
            {
                SP.Execute(SD.DeleteProduct, parameters);
                return Json(new { success = true, message = "Product Deleted Successfully!" });
            }
        }

        #endregion
    }
}
