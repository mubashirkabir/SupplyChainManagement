using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class EmployeeSalary
    {

        [Key]
        [Required]
        public int Sno { get; set; }

        [Required]
        [MaxLength(12)]
       
        public string Id { get; set; }

        
    
        [Required]
        [Display(Name = "Enter Name of the Month")]
        public string Month { get; set; }
        [Required]
        [Display(Name = "Enter Number of Working Days")]
        public string WDays { get; set; }

        [Required]
        [Display(Name = "Enter Allowance")]
        public string Allowance { get; set; }

        [Required]
        public string Total { get; set; }

        [Required]
        public string Year { get; set; }





    }
}