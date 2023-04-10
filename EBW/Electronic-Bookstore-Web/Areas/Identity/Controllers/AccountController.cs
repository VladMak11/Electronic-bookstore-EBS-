using EBW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utility;
using NovaPoshtaApi;
using EBW.Models.Validators;
using FluentValidation;

namespace Electronic_Bookstore_Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Register() 
        {
            if (!_roleManager.RoleExistsAsync(Role.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Admin)).GetAwaiter().GetResult();
            }

            //NovaPoshtaService novaPoshtaService = new NovaPoshtaService(_config);
            //var request = new SettlementsRequest()
            //{
            //    CityName = "київ",
            //    Limit = "50",
            //    Page = "1"
            //};
            //await novaPoshtaService.GetCitiesAsync(request);

            return View(new InputModelforRegistration());
        }
        

        [HttpPost]
        public async Task<IActionResult> Register(InputModelforRegistration registerIM)
        {
            var localvalidator = new InputModelforRegistrationValidator();
            var result = localvalidator.Validate(registerIM);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var searchUser = await _userManager.FindByEmailAsync(registerIM.Email);
            if (searchUser != null)
            {
                ModelState.AddModelError(nameof(registerIM.Email), "It looks like there is already an account registered with this email.");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = registerIM.Email,
                    UserName = registerIM.Email,
                    LastName= registerIM.LastName,
                    FirstName= registerIM.FirstName,
                    PhoneNumber = registerIM.PhoneNumber,
                };
                var resultUserResponce = await _userManager.CreateAsync(user, registerIM.Password);

                if (resultUserResponce.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role.Role_Customer);
                    return View("SuccessRegistration");
                }
                else
                {
                    ModelState.AddModelError(nameof(registerIM.Password), "The password does not meet the requirements.");
                    return View(registerIM);
                }
            }

            return View(registerIM);
        }

        [HttpGet]
        public IActionResult Login() => View(new InputModelforLogin());

        [HttpPost]
        public async Task<IActionResult> Login(InputModelforLogin logimIM)
        {
            var localvalidator = new InputModelforLoginValidator();
            var resultValidate = localvalidator.Validate(logimIM);
            foreach (var error in resultValidate.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var user = await _userManager.FindByEmailAsync(logimIM.Email);
            if(user == null)
            {
                ModelState.AddModelError(nameof(logimIM.Email), "The account with this email does not exist.");
            }
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var securityCheck = await _userManager.CheckPasswordAsync(user, logimIM.Password);
                    if (securityCheck)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, logimIM.Password, false, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
                        }
                    }
                    ModelState.AddModelError(nameof(logimIM.Password), "Incorrect password.");
                    return View(logimIM);
                }
            }

            return View(logimIM);
        }
    
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }
    }
}
