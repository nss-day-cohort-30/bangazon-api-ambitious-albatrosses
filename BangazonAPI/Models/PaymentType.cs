using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class PaymentType
    {
        public int Id { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [Required]
        [StringLength(55)]
        public string Name { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}