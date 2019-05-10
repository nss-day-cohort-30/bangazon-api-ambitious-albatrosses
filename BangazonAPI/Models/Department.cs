using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    //lol hi
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string Name { get; set; }

        [Required]
        public int Budget { get; set; }
    }
}