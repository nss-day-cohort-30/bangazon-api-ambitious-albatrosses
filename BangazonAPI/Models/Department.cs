// Author: Warner Carpenter
// Purpose: This class is the model that defines the department resource and contains all the properties of departments

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string Name { get; set; }

        [Required]
        public int Budget { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();

    }
}