// Author: Connor Bailey
// Purpose: This class is the model that defines the customer resource and contains all the properties of customers

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(55)]
        public string LastName { get; set; }
    }
}