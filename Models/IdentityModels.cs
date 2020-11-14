using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SupplyChainManagement.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionRawMaterial> ProductionRawMaterial { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UsersDetail> UsersDetails { get; set; }
        public DbSet<VendorDetail> VendorDetails { get; set; }
        public DbSet<VendorOrder> VendorOrders { get; set; }
        public DbSet<VendorOrderDetail> VendorOrderDetails { get; set; }
        public DbSet<WarehouseOrder> WarehouseOrders { get; set; }
        public DbSet<WarehouseOrderDetail> WarehouseOrderDetails { get; set; }
        public DbSet<WarehouseOrderType> WarehouseOrderTypes { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
        public DbSet<WarehouseStock> WarehouseStocks { get; set; }
        public DbSet<WarehouseVendorRequest> WarehouseVendorRequests { get; set; }
        public DbSet<WarehouseCompletedOrders> WarehouseCompletedOrderses { get; set; }
        public DbSet<EmployeeManagement> EmployeeManagements { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }




        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}