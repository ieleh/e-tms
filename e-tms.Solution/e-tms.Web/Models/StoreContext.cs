using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace e_tms.Web.Models
{
    public class eTmsContext:DataContext
    {
        public eTmsContext()
            : base("Name=DefaultConnection")
        { 
        }
         
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }

        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderDetail> OrderDetails { get; set; }

        //public System.Data.Entity.DbSet<e_tms.Web.Models.Company> Companies { get; set; }

        //public System.Data.Entity.DbSet<e_tms.Web.Models.Department> Departments { get; set; }

        //public System.Data.Entity.DbSet<e_tms.Web.Models.Work> Works { get; set; }

        //public System.Data.Entity.DbSet<e_tms.Web.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<e_tms.Web.Models.BaseCode> BaseCodes { get; set; }
        public System.Data.Entity.DbSet<e_tms.Web.Models.CodeItem> CodeItems { get; set; }

        public System.Data.Entity.DbSet<e_tms.Web.Models.MenuItem> MenuItems { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        public DbSet<DataTableImportMapping> DataTableImportMappings { get; set; }
    }
}