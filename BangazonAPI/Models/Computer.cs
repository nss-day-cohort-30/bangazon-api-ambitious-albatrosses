// Author: Stephen Clark
// Purpose: This class is the model that defines the computer resource and contains all the properties of computers

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        public DateTime DecomissionDate { get; set; }

        [Required]
        [StringLength(55)]
        public string Make { get; set; }


        [Required]
        [StringLength(55)]

        public string Manufacturer { get; set; }

        
    }
    
}


