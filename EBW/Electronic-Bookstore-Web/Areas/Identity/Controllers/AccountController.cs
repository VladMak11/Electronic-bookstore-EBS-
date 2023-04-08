using EBW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace Electronic_Bookstore_Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register() 
        {
            if (!_roleManager.RoleExistsAsync(Role.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Admin)).GetAwaiter().GetResult();
            }
            return View(new InputModelforRegistration());
        }
        

        [HttpPost]
        public async Task<IActionResult> Register(InputModelforRegistration registerIM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = registerIM.Email,
                    UserName = registerIM.Email
                };
                var resultUserResponce = await _userManager.CreateAsync(user, registerIM.Password);

                if (resultUserResponce.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role.Role_Customer);
                }
                else
                {
                    ModelState.AddModelError(nameof(registerIM.Password), "Пароль не відповідає вимогам");
                    return View(registerIM);
                }
            }
            return View(registerIM);
        }
        public IActionResult Login() => View(new InputModelforLogin());
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
