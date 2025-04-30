using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    //[AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                // Registration logic here
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    PersonName = registerDTO.PersonName,
                    PhoneNumber = registerDTO.Phone,
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
                if(result.Succeeded)
                {
                    if(registerDTO.UserType ==  Core.Enums.UserTypeOptions.Admin)
                    {
                        // Assign user into Admin role
                        if(await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) == null)
                        {
                            await _roleManager.CreateAsync(new ApplicationRole() { Name=UserTypeOptions.Admin.ToString() });
                        }
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                    }
                    else
                    {
                        // Assign user into User role
                        if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) == null)
                        {
                            await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.User.ToString() });
                        }
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                    }
                    // Sign in the user or redirect to a confirmation page
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(PersonsController.Index), "Persons");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("Register", error.Description);
                    }
                    return View(registerDTO);
                }
            }
            else
            {
                ViewBag.Erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(registerDTO);
            }        
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl) // Return Url routes user to the page 
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(loginDTO);
            }
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //Admin
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if(user != null)
                {
                    if(await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new {area="Admin"});
                    }
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid login attempt.");
                return View(loginDTO);
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
