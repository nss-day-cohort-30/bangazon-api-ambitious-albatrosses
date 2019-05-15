using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int PaymentTypeId { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}