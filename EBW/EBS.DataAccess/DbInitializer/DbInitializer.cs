using EBW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace EBW.DataAccess
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDBContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) 
            {
               
            }
            if(!_roleManager.RoleExistsAsync(Role.Role_Customer).GetAwaiter().GetResult())
            {
                //create roles if they aren't created.
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Admin)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    LastName = "LastNameAdmin",
                    FirstName = "FirstNameAdmin"
                },"Admin123").GetAwaiter().GetResult();

                ApplicationUser newUser = _userManager.FindByEmailAsync("admin@gmail.com").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(newUser, Role.Role_Admin).GetAwaiter().GetResult();
            }
        }
    }
}
