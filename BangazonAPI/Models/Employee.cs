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
        [Range(0, 1)]
        public int IsSuperVisor { get; set; }
    }
}