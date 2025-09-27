using Ecommerce_App.Data;
using Ecommerce_App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace Ecommerce_App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ReturnRefundController : Controller
    {

        private readonly ApplicationDbContext _context;
        public ReturnRefundController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }



        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var orders= _context.OrderHeaders.Where(o=>o.ApplicationUserId == claims.Value /*&& o.OrderStatus == SD.OrderStatusApproved*/).OrderByDescending(o=>o.orderDate).ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult ApplyRefund(int id)
        {
            var order = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();

          
            if ((DateTime.Now - order.orderDate).TotalDays > 7)
            {
                TempData["Message"] = "Refund not allowed after 24 hours!";
                return RedirectToAction("Index");
            }

           
            if (!string.IsNullOrEmpty(order.TransactionId))
            {
                var refundService = new RefundService();
                
                refundService.Create(new RefundCreateOptions
                {
                    Charge = order.TransactionId
                });

                if (string.IsNullOrEmpty(order.TransactionId))
                {
                    TempData["Message"] = "No payment exists for this order, so refund cannot be processed.";
                    return RedirectToAction("Index");
                }

                order.OrderStatus = SD.OrderStatusCancelled;
                order.PaymentStatus = SD.PaymentStatusRefunded;
                _context.SaveChanges();

                TempData["Message"] = "Order cancelled and refunded successfully!";
            }

            return RedirectToAction("Index");
        }
    
}

    }

