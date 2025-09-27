using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        
        public string City { get; set; }    
        [Required]
        public string State { get; set; }
        public string StreetAddress { get; set; }
       
        [Display(Name  = "Postal Code")]
        public string PostalCode { get; set; }

        public string Role { get; set; }
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [NotMapped]
        public Company Company { get; set; }
        

    }
}
