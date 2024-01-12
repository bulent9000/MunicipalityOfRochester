using Microsoft.AspNetCore.Mvc;
using Municipality.Data.Repository.IRepository;
using Municipality.Model;

namespace MunicipalityOfRochester.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DistinctMunicipalityController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public DistinctMunicipalityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<DistrictMunicipality> DistrictMunicipality = _unitOfWork.DistrictMunicipality.GetAll();
            return View(DistrictMunicipality);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DistrictMunicipality districtMunicipality)
        {

            _unitOfWork.DistrictMunicipality.Add(districtMunicipality);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }






    }
}
