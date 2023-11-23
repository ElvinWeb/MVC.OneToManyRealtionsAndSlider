using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.OneToManyRealtions.Models;
using MVC.SliderFrontToBack.Helpers;
using MVC.SliderFrontToBack.Models;
using MVC.SliderFrontToBack.ViewModel;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DashBoardController : Controller
    {
        private readonly ProjectDbContext _DbContext;
        private readonly IWebHostEnvironment _env;
        private HomeViewModel HomeViewModel = new HomeViewModel();

        public DashBoardController(ProjectDbContext context, IWebHostEnvironment env)
        {
            _DbContext = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> Sliders = _DbContext.Sliders.ToList();

            return View(Sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slide)
        {
            string fileName = string.Empty;
            if (slide.Image != null)
            {
                fileName = slide.Image.FileName;
                if (slide.Image.ContentType != "image/png" && slide.Image.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("Image", "please select correct file type");
                }

                if (slide.Image.Length > 1048576)
                {
                    ModelState.AddModelError("Image", "file size should be more lower than 1mb ");
                }

                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
            }
            if (!ModelState.IsValid) return View(slide);

            string folder = "bg-slider-images/";
            string rootPath = _env.WebRootPath;

            Helper.PathCombine(rootPath, folder, slide.Image);

            //fileName = Guid.NewGuid().ToString() + fileName;


            //string path = $"C:\\Users\\II novbe\\Desktop\\all task\\MVC.OneToManyRealtionsAndSlider\\WebApplication\\MVC.OneToManyRealtions\\wwwroot\\assets\\bg-slider-images\\{fileName}";

            //using (FileStream fileStream = new FileStream(path, FileMode.Create))
            //{
            //    slide.Image.CopyTo(fileStream);
            //}

            slide.ImgUrl = fileName;

            _DbContext.Sliders.Add(slide);
            _DbContext.SaveChanges();


            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            Slider slide = _DbContext.Sliders.FirstOrDefault(s => s.Id == id);
            return View(slide);
        }

        [HttpPost]
        public IActionResult Update(Slider slide)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Slider slide = _DbContext.Sliders.FirstOrDefault(s => s.Id == id);
            return View(slide);
        }

        [HttpPost]
        public IActionResult Delete(Slider slide)
        {
            Slider wantedSlide = _DbContext.Sliders.FirstOrDefault(s => s.Id == slide.Id);

            string fileName = wantedSlide.ImgUrl;
            string path = $"C:\\Users\\II novbe\\Desktop\\all task\\MVC.OneToManyRealtionsAndSlider\\WebApplication\\MVC.OneToManyRealtions\\wwwroot\\assets\\bg-slider-images\\{fileName}";

            if (wantedSlide.Image != null)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            _DbContext.Sliders.Remove(wantedSlide);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
