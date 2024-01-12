using Microsoft.AspNetCore.Mvc;
using MunicipalityOfRochester.Models;
using System.Diagnostics;

namespace MunicipalityOfRochester.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

       
    }
}