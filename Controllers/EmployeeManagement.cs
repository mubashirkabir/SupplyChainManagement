using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Mvc;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;

namespace SupplyChainManagement.Controllers
{
    public class EmployeeManagementController : Controller
    {
        private ApplicationDbContext _context;

        public EmployeeManagementController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }

        // GET: EmployeeManagement
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["userrole"].ToString()) != 5)
                {
                    Response.Redirect("~/Home/Index");
                }

            }
            else
            {
                Response.Redirect("~/Home/Index");

            }
            var EDetail = _context.EmployeeManagements.ToList();

            return View(EDetail);
        }

        public ActionResult AddEmployee()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult AddEmployee(EmployeeAdd Item)
        {
            
            var add = new EmployeeManagement();
            


            add = Item.employeeManagement;
            add.Position = Request.Form["position"];

            _context.EmployeeManagements.Add(add);



            _context.SaveChanges();
            return Redirect("Index");
        }


        [System.Web.Mvc.Route("EmployeeManagement/EmployeeEdit/{id}")]
        public ActionResult EmployeeEdit(string id)
        {
           
                var EmployeInDb = _context.EmployeeManagements.SingleOrDefault(e => e.Id == id);
                if (EmployeInDb == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

            EmployeeAdd employeeAdd = new EmployeeAdd();
            employeeAdd.employeeManagement = EmployeInDb;
            return View(employeeAdd);
        }

      
        public ActionResult EmployeeUpdate(EmployeeAdd employeeAdd)
        {
            var addEmployee = _context.EmployeeManagements.SingleOrDefault(e => e.Id == employeeAdd.employeeManagement.Id);
            addEmployee.Name = employeeAdd.employeeManagement.Name;
            addEmployee.LastName = employeeAdd.employeeManagement.LastName;
            addEmployee.JoiningDate = employeeAdd.employeeManagement.JoiningDate;
            addEmployee.Address = employeeAdd.employeeManagement.Address;
            addEmployee.Contact = employeeAdd.employeeManagement.Contact;
            addEmployee.Position = employeeAdd.employeeManagement.Position;
            _context.SaveChanges();

            return Redirect("~/EmployeeManagement/Index");

        }


        public ActionResult SalaryDetail()
        {
            var EDetail = _context.EmployeeSalaries.ToList();
            var employee = _context.EmployeeManagements.ToList();
            var employeesalary = (from s in EDetail
                                  join e in employee on s.Id equals e.Id
                                  select new EmployeeAdd()
                                  {
                                      employeeManagement = e,
                                      EmployeeSalary = s
                                  }).ToList();
            return View(employeesalary);
           
        }


        public ActionResult CalculateSalary ()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CalculateAddedSalary(EmployeeAdd Item)
        {
            bool check = false;
            List<EmployeeSalary> employees = _context.EmployeeSalaries.ToList();
            for (int i = 0; i < employees.Count(); i++) {
                var EmpID = Item.employeeManagement.Id;
                var Month = Request.Form["month"];
                var Year = Request.Form["year"];


                if ((employees[i].Id.Equals(EmpID) && (employees[i].Month.Equals(Month)&& (employees[i].Year.Equals(Year)))))
                {
                    
                    check = true;
                }
            }
            var add = new EmployeeSalary();
            
            if (check == false)
            {
                var emplID = new EmployeeSalary();

                emplID.Id = Item.employeeManagement.Id;
                emplID.Month = Request.Form["month"];
                emplID.Year = Request.Form["year"];
                emplID.Allowance = Request.Form["allowance"];
                emplID.WDays = Request.Form["Wdays"];
                try
                {
                    var checkPosition = Item.employeeManagement.Position;
                    if (checkPosition.Equals("Manager"))
                    {
                        float calculation1 = Int32.Parse(emplID.WDays) * 1800;
                        float TotalSalary1 = Int32.Parse(emplID.Allowance) + calculation1;
                        emplID.Total = TotalSalary1.ToString();
                    }
                    else if (checkPosition.Equals("Worker"))
                    {
                        float calculation1 = Int32.Parse(emplID.WDays) * 1000;
                        float TotalSalary1 = Int32.Parse(emplID.Allowance) + calculation1;
                        emplID.Total = TotalSalary1.ToString();
                    }

                    else if (checkPosition.Equals("Manager"))
                    {
                        float calculation1 = Int32.Parse(emplID.WDays) * 2500;
                        float TotalSalary1 = Int32.Parse(emplID.Allowance) + calculation1;
                        emplID.Total = TotalSalary1.ToString();
                    }
                    else
                    {

                        return Redirect("Index");
                    }

               

                //     float calculation = Int32.Parse(emplID.WDays) * 1000;
            //    float TotalSalary = Int32.Parse(emplID.Allowance) + calculation;
            //    emplID.Total = TotalSalary.ToString();

                _context.EmployeeSalaries.Add(emplID);
                _context.SaveChanges();

                }
                catch (Exception e)
                {
                    return Redirect("Index");
                }

            }
              
         
            _context.SaveChanges();
            return Redirect("~/EmployeeManagement/SalaryDetail");

        }


        public ActionResult SalaryCalculation(EmployeeAdd employeeAdd)
        {
            var addEmployee = _context.EmployeeManagements.SingleOrDefault(e => e.Id == employeeAdd.employeeManagement.Id);
            addEmployee.Name = employeeAdd.employeeManagement.Name;
            addEmployee.LastName = employeeAdd.employeeManagement.LastName;
            addEmployee.JoiningDate = employeeAdd.employeeManagement.JoiningDate;
            addEmployee.Address = employeeAdd.employeeManagement.Address;
            addEmployee.Contact = employeeAdd.employeeManagement.Contact;
            addEmployee.Position = employeeAdd.employeeManagement.Position;
            _context.SaveChanges();

            return Redirect("~/EmployeeManagement/Index");

        }

        [System.Web.Mvc.Route("EmployeeManagement/CalculateSalary/{id}")]
        public ActionResult CalculateSalary(string id)
        {

            var EmployeInDb = _context.EmployeeManagements.SingleOrDefault(e => e.Id == id);
            if (EmployeInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            EmployeeAdd employeeAdd = new EmployeeAdd();
            employeeAdd.employeeManagement = EmployeInDb;
            return View(employeeAdd);
        }

    }



}
