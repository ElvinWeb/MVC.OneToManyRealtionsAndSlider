using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.SliderFrontToBack.Models;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManufacturerController : Controller
    {
        private readonly ProjectDbContext _DbContext;

        public ManufacturerController(ProjectDbContext context)
        {
            _DbContext = context;

        }
        public IActionResult Index()
        {
            List<Manufacturer> manufacturers = _DbContext.Manufacturers.ToList();

            return View(manufacturers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Manufacturer manufacturer)
        {
            if (!ModelState.IsValid) return View();

            if (_DbContext.Manufacturers.Any(a => a.Name.ToLower() == manufacturer.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Manufacturer has already created!");
                return View();
            }

            _DbContext.Manufacturers.Add(manufacturer);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();

            Manufacturer manufacturer = _DbContext.Manufacturers.FirstOrDefault(a => a.Id == id);

            if (manufacturer == null) return NotFound();

            return View(manufacturer);
        }

        [HttpPost]
        public IActionResult Update(Manufacturer manufacturer)
        {
            if (!ModelState.IsValid) return View();

            Manufacturer existManufacturer = _DbContext.Manufacturers.FirstOrDefault(a => a.Id == manufacturer.Id);

            if (existManufacturer == null) return NotFound();

            if (_DbContext.Manufacturers.Any(a => a.Id != manufacturer.Id && a.Name.ToLower() == manufacturer.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Manufacturer has already created!");
                return View();
            }

            existManufacturer.Name = manufacturer.Name;

            _DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Manufacturer manufacturer = _DbContext.Manufacturers.FirstOrDefault(a => a.Id == id);
            return View(manufacturer);
        }

        [HttpPost]
        public IActionResult Delete(Manufacturer manufacturer)
        {

            Manufacturer existManufacturer = _DbContext.Manufacturers.FirstOrDefault(a => a.Id == manufacturer.Id);

            if (existManufacturer == null) return NotFound();


            _DbContext.Manufacturers.Remove(existManufacturer);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
