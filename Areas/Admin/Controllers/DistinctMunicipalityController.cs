using Microsoft.AspNetCore.Mvc;
using Municipality.Data.Repository.IRepository;
using Municipality.Model;
using System.Drawing.Printing;

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

      

        [HttpPost]
        public IActionResult Create(DistrictMunicipality districtMunicipality)
        {

            _unitOfWork.DistrictMunicipality.Add(districtMunicipality);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }

     
        [HttpPost]
        public IActionResult Edit(DistrictMunicipality districtMunicipality)
        {

            _unitOfWork.DistrictMunicipality.Update(districtMunicipality);
            _unitOfWork.Save();
            return RedirectToAction("Index");


        }
        public IActionResult Delete(int id)
        {

            var districtMunicipality=_unitOfWork.DistrictMunicipality.GetFirstOrDefault(k => k.DistMunId == id);

            _unitOfWork.Save();
            return RedirectToAction("Index");



        }
    }
}
