using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.Utility
{
    public static class SD
    {
        //Roles 
        public const string Role_Admin      = "Admin_User";
        public const string Role_Employee   = "Employee_User";
        public const string Role_Company    = "Company_User";
        public const string Role_Individual = "Individual_User";


        //Categories
        public const string CreateCategory  = "SP_CreateCategory";
        public const string GetCategories   = "SP_GetCategories";
        public const string UpdateCategory  = "SP_UpdateCategory";
        public const string DeleteCategory  = "SP_DeleteCategory";
        public const string GetCategory     = "SP_GetCategory";
                    
        //CoverTypes

        public const string CreateCoverType = "SP_Createcovertypes";
        public const string GetCoverTypes   = "SP_Getcovertypes";
        public const string UpdateCoverType = "SP_Updatecovertypes";
        public const string DeleteCoverType = "SP_Deletecovertypes";
        public const string GetCoverType    = "SP_Getcovertype";

        //Products
        public const string CreateProduct   = "SP_CreateProduct";
        public const string UpdateProduct   = "SP_UpdateProduct";
        public const string DeleteProduct   = "SP_DeleteProduct";
        public const string GetProducts     = "SP_GetProducts";
        public const string GetProduct      = "SP_GetProduct";

        //Companies
        public const string CreateCompany   = "SP_CreateCompany";
        public const string UpdateCompany   = "SP_UpdateCompany";
        public const string DeleteCompany   = "SP_DeleteCompany";
        public const string GetCompanies    = "SP_GetCompanies";
        public const string GetCompany      = "SP_GetCompany";

        //shoppingcart
        public const string GetAllCartsByUserId         = "SP_GetAllShoppingCartsByUser";
        public const string GetOneCartByCartId          = "SP_GetOneShoppingCartById";
        public const string SS_ShoppingCartCountSession = "CartCountSession";
        public const string AddOrUpdateShoppingCart     = "SP_AddorUpdateShoppingCart";

        //OrderHeader
        public const string GetAllOrderHeaderDetails    = "GetAllOrderHeader";

        //OrderStatus
        public const string OrderStatusInProgress       = "Processing";
        public const string OrderStatusShipped          = "Shipped";
        public const string OrderStatusCancelled        = "Cancelled";
        public const string OrderStatusApproved         = "Approved";
        public const string OrderStatusRefunded         = "Refunded";
        public const string OrderStatusPending          = "Pending";

        //PaymentStatus
        public const string PaymentStatusPending        = "Pending";
        public const string PaymentStatusRejected       = "Rejected";
        public const string PaymentStatusDelayPayment   = "Delay";
        public const string PaymentStatusApproved       = "Approved";
        public const string PaymentStatusRefunded       = "Refunded";





        public static double GetPriceBasedonQuantity(double price, double price50, double price100, double quantity)
        {
            if (quantity < 50)
            {
                return price;
            }
            else if(quantity <100){
                return price50;
            }
            else
            {
                return price100;
            }
        }
        
       
    }
    }


    

