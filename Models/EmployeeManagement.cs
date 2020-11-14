using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class EmployeeManagement
    {
        [Key]
        [Required(ErrorMessage = "Please enter Employee ID")]
        [MaxLength(12)]
        [Display(Name = "Enter Employee ID")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Enter Employee Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Enter Employee LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Enter Employee Position")]
        public string Position { get; set; }

        [Required]
        [Display(Name = "Enter Employee JoiningDate")]
        public string JoiningDate { get; set; }

        [Required]
        [Display(Name = "Enter Employee Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Enter Employee Contact")]
        public string Contact { get; set; }


    }
}