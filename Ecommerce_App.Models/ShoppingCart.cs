using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
       
        public int Count { get; set; }
        public int Id { get; set; }
        [NotMapped]
        public double Price { get; set; }
        public string ApplicationUserId {get; set;}
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId {  get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
