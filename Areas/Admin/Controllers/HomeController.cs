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
        public IActionResult Pricing()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


    }
}