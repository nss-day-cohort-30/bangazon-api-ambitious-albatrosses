// Author: Kirren Covey
// Purpose: This class is the model that defines the product type resource and contains all the properties of product types

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BangazonAPI.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(55)]
        public string Name { get; set; }
    }
}