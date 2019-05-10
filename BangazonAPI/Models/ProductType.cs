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