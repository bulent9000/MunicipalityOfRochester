using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Municipality.Data.Repository.IRepository;
using Municipality.Model.ViewModels;

namespace MunicipalityOfRochester.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _hostEnvironment;
        public EmployeesController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;




        }
        public IActionResult Index()
        {
            var employeesList = _unitOfWork.Employees.GetAll();
            return View(employeesList);
        }


        [HttpGet]
        public IActionResult Crup(int? id = 0)
        {
            EmployeesVM employeesVM = new()
            {
                Employees = new(),
                Managers = _unitOfWork.Managers.GetAll().Select(i => new SelectListItem
                {
                    Text = i.ManagerId.ToString(),
                    Value = i.ManagerId.ToString()

                })


            };

            if (id == null || id <= 0)
            {

                return View(employeesVM);


            }

            employeesVM.Employees = _unitOfWork.Employees.GetFirstOrDefault(i => i.EmployeesId == id);

            if (employeesVM.Employees == null)
            {

                return View(employeesVM);

            }

            return View(employeesVM);

        }


        [HttpPost]
        public IActionResult Crup(EmployeesVM employeesVM, IFormFile file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploadRoot = Path.Combine(wwwRootPath, @"img\employee");
                var extention = Path.GetExtension(file.FileName);

                if (employeesVM.Employees.Photo != null)
                {
                    var oldPictPath = Path.Combine(wwwRootPath, employeesVM.Employees.Photo);
                    if (System.IO.File.Exists(oldPictPath))
                    {
                        System.IO.File.Delete(oldPictPath);
                    }


                }


                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extention), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    employeesVM.Employees.Photo = @"\img\employee\" + fileName + extention;

                }



            }

            if (employeesVM.Employees.EmployeesId <= 0)
            {

                _unitOfWork.Employees.Add(employeesVM.Employees);

            }
            else
            {

                _unitOfWork.Employees.Update(employeesVM.Employees);
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

                var employees = _unitOfWork.Employees.GetFirstOrDefault(i => i.EmployeesId == id);
                _unitOfWork.Employees.Remove(employees);
                _unitOfWork.Save();
                return RedirectToAction("Index");


            }
        }

    }
}
