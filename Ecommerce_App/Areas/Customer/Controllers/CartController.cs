using Dapper;
using Ecommerce_App.Data;
using Ecommerce_App.DataAccess.Repository;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Ecommerce_App.Models;
using Ecommerce_App.Models.ViewModels;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;



namespace Ecommerce_App.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class CartController : Controller
    {

        private readonly ISP_CALL SP;
        private readonly ApplicationDbContext _context;
        private readonly TwilioService _twilio;

        public CartController(ISP_CALL sP_CALL, ApplicationDbContext applicationDbContext, TwilioService twilio)
        {
            SP = sP_CALL;
            _context = applicationDbContext;
            _twilio = twilio;
        }

        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
           

            if(claims == null)
            {
                shoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(shoppingCartVM);
            }

            shoppingCartVM = new ShoppingCartVM
            {
                ListCart = _context.ShoppingCarts.Include(s => s.Product).Where(sc => sc.ApplicationUserId == claims.Value),
                OrderHeader = new()
            };

            var Count = shoppingCartVM.ListCart.Count();
            HttpContext.Session.SetInt32(SD.SS_ShoppingCartCountSession, Count);

            shoppingCartVM.OrderHeader.OrderTotal = 0;

            shoppingCartVM.OrderHeader.ApplicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == claims.Value);
            foreach (var list in shoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedonQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                shoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);

                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }

            return View(shoppingCartVM);
        }
        
        [HttpGet]
        public IActionResult GetAll( )
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            DynamicParameters param = new();
            param.Add("@ApplicationUserId", claim.Value);
            var all = SP.List<ShoppingCart>(SD.GetAllCartsByUserId,param);
            return Json( new{ all});
        }

       
        public IActionResult Delete(int id)
        {
            var something = _context.ShoppingCarts.FirstOrDefault(a=>a.Id == id);   
            if (something == null)
            {
                return NotFound();
            }
            else
            {
                _context.ShoppingCarts.Remove(something);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
        

        public IActionResult Test()
        {
            HttpContext.Session.SetInt32(SD.SS_ShoppingCartCountSession, 5);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult plus(int id)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string Userid = claim.Value;

            DynamicParameters param = new();
            param.Add("@Id", id);
            //param.Add("@ApplicationUserId", Userid);


            var cart = SP.OneRecord<ShoppingCart>(SD.GetOneCartByCartId, param);

            if (cart != null)
            {
         
                cart.Count += 1;

            
                DynamicParameters updateParam = new();
                updateParam.Add("@Id", cart.Id);
                updateParam.Add("@Count", cart.Count);

                SP.Execute(SD.AddOrUpdateShoppingCart, updateParam);


                DynamicParameters countParam = new();
                countParam.Add("@ApplicationUserId", Userid);

                int cartCount = SP.OneRecord<int>("SP_GetCartCountByUser", countParam);
                HttpContext.Session.SetInt32(SD.SS_ShoppingCartCountSession, cartCount);
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public IActionResult Summary(List<int> SelectedCartIds)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

      
            shoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _context.ShoppingCarts
            .Include(s => s.Product)
            .Where(a => a.ApplicationUserId == Claims.Value && SelectedCartIds.Contains(a.Id))
            .ToList(),
                OrderHeader = new()
            };
            shoppingCartVM.OrderHeader.ApplicationUser = _context.ApplicationUsers.FirstOrDefault(a => a.Id == Claims.Value);
            
            foreach (var list in shoppingCartVM.ListCart)
            {

                list.Price = SD.GetPriceBasedonQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                shoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);

                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }

            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
           
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult SummaryPost(string stripetoken,List<int> SelectedCartIds)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claims == null) return NotFound();
            
            shoppingCartVM.OrderHeader.ApplicationUser = _context.ApplicationUsers.FirstOrDefault(au => au.Id == claims.Value);

            //DynamicParameters param = new();
            //param.Add("ApplicationUserId", claims.Value);

            //var CartList = SP.ListWithRelated<ShoppingCart,AppProduct>(
            //                            SD.GetAllCartsByUserId,
            //                            (sc, p) => { sc.Product = p; return sc; },
            //                            "ProductId",
            //                            param
            //                        ).ToList();

            shoppingCartVM.ListCart = _context.ShoppingCarts.Include(s =>s.Product).Where(s=>s.ApplicationUser.Id == claims.Value).ToList();

            shoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppingCartVM.OrderHeader.orderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;
            _context.OrderHeaders.Add(shoppingCartVM.OrderHeader);
            _context.SaveChanges();


            foreach (var list in shoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedonQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = list.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price = list.Price,
                    count = list.Count
                };
                shoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
           

            _context.ShoppingCarts.RemoveRange(shoppingCartVM.ListCart);
            _context.SaveChanges();
            HttpContext.Session.SetInt32(SD.SS_ShoppingCartCountSession, 0);

            if (stripetoken == null)
            {
                shoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30).ToString();
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                shoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
            }

            else
            {
                var options = new ChargeCreateOptions()
                {

                    Amount = Convert.ToInt32(shoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "USD",
                    Description = "OrderId:" + shoppingCartVM.OrderHeader.Id.ToString(),
                    Source = stripetoken

                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.BalanceTransactionId == null)
                {
                    shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else
                {
                    shoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                }

                if (charge.Status.ToLower() == "Succeeded")
                {
                    shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    shoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                    shoppingCartVM.OrderHeader.orderDate = DateTime.Now;
                }
                _context.SaveChanges();
            }



            return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.OrderHeader.Id });
            //Pending Code
           
        }
        public IActionResult OrderConfirmation(int id)
        {
            var order = _context.OrderHeaders.Include(s => s.ApplicationUser).FirstOrDefault(o=> o.Id == id);
            if (order == null) return NotFound();

            SendOrderComfirmationEmail(order);
            string message = $"Hi {order.ApplicationUser.Name}, your order #{order.Id} has been confirmed. Total: ${order.OrderTotal}.If You Have Any Issues Check Our Help And Support Page";
            _twilio.sendsms(order.ApplicationUser.PhoneNumber, message);

            return View(new {Order = order});
        }

        private void SendOrderComfirmationEmail(OrderHeader order)
        {
            try
            {
                var fromAddress = new MailAddress("rishavkumargamer@gmail.com", "Book Shopping");
                var toAddress = new MailAddress(order.ApplicationUser.Email, order.ApplicationUser.Name);
                const string fromPassword = "izcw brhq jhwp lalo"; 
                string subject = $"Order Confirmation - Order #{order.Id}";
                string body = $"Hi {order.ApplicationUser.Name},\n\n" +
                              $"Thank you for your order! Your order total is ${order.OrderTotal}.\n" +
                              $"We will notify you once it ships.\n\n" +
                              $"Order Details:\n";

               
                body += "\nThank you for shopping with us!";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
            }

        }
    }
    }
   

    

    

