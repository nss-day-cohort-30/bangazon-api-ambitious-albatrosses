// Author: Warner Carpenter
// Purpose: This class is the model that defines the employee resource and contains all the properties of employees

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(55)]
        public string LastName { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public bool IsSuperVisor { get; set; }

        public Department Department { get; set; }
        public Computer Computer { get; set; }
    }
}