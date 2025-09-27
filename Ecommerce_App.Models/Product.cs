using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required]
        public int ListedPrice { get; set; }
        [Required]
        [Range(1,2000)]
        public int Price { get; set; }
        [Required]
        [Range(1, 2000)]
        public int Price50 { get; set; }
        [Required]
        [Range(1, 2000)]
        public int Price100 {  get; set; }
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId {  get; set; }

        public Category Category { get; set; }
        [Required]
        [DisplayName("CoverType")]
        public int CoverTypeId { get; set; }
        public CoverType CoverType { get; set; }



    }
}
