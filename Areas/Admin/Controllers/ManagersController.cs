using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Municipality.Data.Repository.IRepository;
using Municipality.Model.ViewModels;

namespace MunicipalityOfRochester.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _hostEnvironment;
        public ManagersController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;




        }
        public IActionResult Index()
        {
            var managerList = _unitOfWork.Managers.GetAll();
            return View(managerList);
        }


        [HttpGet]
        public IActionResult Crup(int? id = 0)
        {
            ManagersVM managerVM = new()
            {
                Managers = new(),
                DistrictMunicipality = _unitOfWork.DistrictMunicipality.GetAll().Select(i => new SelectListItem
                {
                    Text = i.DistMunId.ToString(),
                    Value = i.DistMunId.ToString()

                })


            };

            if (id == null || id <= 0)
            {

                return View(managerVM);


            }

            managerVM.Managers = _unitOfWork.Managers.GetFirstOrDefault(i => i.ManagerId == id);

            if (managerVM.Managers == null)
            {

                return View(managerVM);

            }

            return View(managerVM);

        }


        [HttpPost]
        public IActionResult Crup(ManagersVM managerVM, IFormFile file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploadRoot = Path.Combine(wwwRootPath, @"img\manager");
                var extention = Path.GetExtension(file.FileName);

                if (managerVM.Managers.Photo != null)
                {
                    var oldPictPath = Path.Combine(wwwRootPath, managerVM.Managers.Photo);
                    if (System.IO.File.Exists(oldPictPath))
                    {
                        System.IO.File.Delete(oldPictPath);
                    }


                }


                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extention), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    managerVM.Managers.Photo = @"\img\manager\" + fileName + extention;

                }



            }

            if (managerVM.Managers.ManagerId <= 0)
            {

                _unitOfWork.Managers.Add(managerVM.Managers);

            }
            else
            {

                _unitOfWork.Managers.Update(managerVM.Managers);
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }


        public IActionResult Delete(int? id)
        {

            if (id == null || id <= 0)
            {
                return NotFound();

            }

            else
            {

                var manager = _unitOfWork.Managers.GetFirstOrDefault(i => i.ManagerId == id);
                _unitOfWork.Managers.Remove(manager);
                _unitOfWork.Save();
                return RedirectToAction("Index");


            }
        }

    }
}
