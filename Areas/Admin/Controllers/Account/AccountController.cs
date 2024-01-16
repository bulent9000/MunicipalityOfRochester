using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Municipality.Data;
using Municipality.Model.ViewModels;
using System.Security.Claims;
using Municipality.Model.Account;

namespace MunicipalityOfRochester.Areas.Admin.Controllers.Account
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public readonly AppDbContext db;
        public AccountController(AppDbContext db)
        {
            this.db = db;       
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {

                var data = db.Users.Where(i => i.UserName == model.UserName).SingleOrDefault();
                if (data != null)
                {
                    bool isValid=(data.UserName==model.UserName && data.Password==model.Password);

                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.UserName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", model.UserName);
                        return RedirectToAction("Index", "Login Gidilecek sayfa Controller");
                    }
                    else
                    {
                        TempData["errorPassword"] = "Invalid password!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["errorUsername"] = "Username not found!";
                    return View(model);
                }


            }
            else
            {
                return View(model);
            }



        }
        public IActionResult LogOut()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);

            }

            return RedirectToAction("Index", "User");

        }

        public IActionResult SignUp()
        {
            return View();
        }

  
        [HttpPost]
        public IActionResult SignUp(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.Mobile,
                    


                };

                db.Users.Add(data);
                db.SaveChanges();
                TempData["successMessage"] = "başarılı!";
                return RedirectToAction("Index", "User");
            }
            else
            {
                TempData["errorMessage"] = "başarısız!";
                return View(model);
            }


        }




    }
}
