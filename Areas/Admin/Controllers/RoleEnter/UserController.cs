using Microsoft.AspNetCore.Mvc;

namespace MunicipalityOfRochester.Areas.Admin.Controllers.RoleEnter
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
