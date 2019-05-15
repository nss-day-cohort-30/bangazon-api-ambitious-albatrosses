// Author: Kirren Covey
// Purpose: This class is the model that defines the product resource and contains all the properties of products

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BangazonAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}