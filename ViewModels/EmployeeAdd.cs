using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.ViewModels
{
    public class EmployeeAdd
    {
        [Required]
        public EmployeeManagement employeeManagement { get; set; }
        public EmployeeSalary EmployeeSalary { get; set; }



    }
}