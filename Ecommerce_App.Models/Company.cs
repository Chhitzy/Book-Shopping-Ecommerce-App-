using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
      
        public string City { get; set; }
        [Display(Name ="Street Address")]
       
        public string StreetAddress { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string State { get; set; }
        [Required]
        public bool IsAuthorizedCompany {  get; set; }
        [Required]
        [Display(Name= "Phone Number")]
        public string PhoneNumber { get; set; }

    }
}
