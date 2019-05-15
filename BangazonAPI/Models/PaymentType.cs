// Author: Connor Bailey
// Purpose: This class is the model that defines the payment type resource and contains all the properties of payment types

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