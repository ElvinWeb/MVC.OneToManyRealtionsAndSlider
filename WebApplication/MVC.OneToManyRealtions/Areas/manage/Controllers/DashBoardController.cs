using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.OneToManyRealtions.Models;
using MVC.SliderFrontToBack.Helpers;
using MVC.SliderFrontToBack.Models;
using MVC.SliderFrontToBack.ViewModel;
using System.Drawing;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DashBoardController : Controller
    {
        private readonly ProjectDbContext _DbContext;
        private readonly IWebHostEnvironment _env;
        private HomeViewModel _HomeVM = new HomeViewModel();

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
            if (!ModelState.IsValid) return View(slide);

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
            else
            {
                ModelState.AddModelError("Image", "Must be choosed!!");
                return View();
            }


            string folder = "assets/bg-slider-images";

            string newFileName = Helper.GetFileName(_env.WebRootPath, folder, slide.Image);

            slide.ImgUrl = newFileName;

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

            Slider wantedSlide = _DbContext.Sliders.FirstOrDefault(s => s.Id == slide.Id);

            if (wantedSlide == null) return NotFound();

            if (!ModelState.IsValid) return View();

            string fileName = string.Empty;

            if (slide.Image != null)
            {
                fileName = slide.Image.FileName;

                if (slide.Image.ContentType != "image/png" && slide.Image.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("Image", "please select correct file type");
                    return View(slide);
                }

                if (slide.Image.Length > 1048576)
                {
                    ModelState.AddModelError("Image", "file size should be more lower than 1mb ");
                    return View(slide);
                }

                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                string folderPath = "assets/bg-slider-images";
                string expiredFileName = Helper.GetFileName(_env.WebRootPath, folderPath, slide.Image);

                string wantedPath = Path.Combine(_env.WebRootPath, folderPath, wantedSlide.ImgUrl);

                if (System.IO.File.Exists(wantedPath))
                {
                    System.IO.File.Delete(wantedPath);
                }

                wantedSlide.ImgUrl = expiredFileName;
            }

            wantedSlide.Title = slide.Title;
            wantedSlide.Description = slide.Description;
            wantedSlide.SubTitle = slide.SubTitle;
            wantedSlide.Name = slide.Name;
            wantedSlide.RedirectUrl = slide.RedirectUrl;



            _DbContext.SaveChanges();
            return RedirectToAction("Index");
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

            string folderPath = "assets/bg-slider-images";


            if (wantedSlide.ImgUrl != null)
            {
                string path = Path.Combine(_env.WebRootPath, folderPath, wantedSlide.ImgUrl);

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
