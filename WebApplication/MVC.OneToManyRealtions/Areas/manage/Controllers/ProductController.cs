using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.SliderFrontToBack.Models;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly ProjectDbContext _DbContext;

        public ProductController(ProjectDbContext context)
        {
            _DbContext = context;

        }
        public IActionResult Index()
        {
            List<Product> products = _DbContext.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            if (!ModelState.IsValid) return View();

            if (!_DbContext.Manufacturers.Any(a => a.Id == product.ManufacturerId))
            {
                ModelState.AddModelError("ManufacturerId", "Manufacturer is not found!");
                return View();
            }

            bool check = false;
            if (product.TagIds != null)
            {
                foreach (var tagId in product.TagIds)
                {
                    if (!_DbContext.Tags.Any(p => p.Id == tagId))
                    {
                        check = true;
                        break;
                    }

                }
            }
            if (check)
            {
                ModelState.AddModelError("TagId", "Tag id not found!");
                return View();
            }
            else
            {
                if (product.TagIds != null)
                {
                    foreach (var tagId in product.TagIds)
                    {
                        ProductTag productTag = new ProductTag()
                        {
                            Product = product,
                            TagId = tagId,
                        };
                        _DbContext.ProductTags.Add(productTag);
                    }
                }
            }

            _DbContext.Products.Add(product);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            if (id == null) return NotFound();

            Product product = _DbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();


            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            Product wantedProduct = _DbContext.Products.FirstOrDefault(p => p.Id == product.Id);

            if (wantedProduct == null)
            {
                return NotFound();
            }

            _DbContext.Products.Remove(wantedProduct);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            if (id == null) return NotFound();

            Product exitProduct = _DbContext.Products.Include(pt => pt.ProductTags).FirstOrDefault(b => b.Id == id);

            if (exitProduct == null) return NotFound();

            exitProduct.TagIds = exitProduct.ProductTags.Select(t => t.TagId).ToList();

            return View(exitProduct);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {

            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            if (!ModelState.IsValid) return View();

            Product existProduct = _DbContext.Products.Include(pt => pt.ProductTags).FirstOrDefault(b => b.Id == product.Id);

            if (existProduct == null) return NotFound();

            if (!_DbContext.Manufacturers.Any(a => a.Id == product.ManufacturerId))
            {
                ModelState.AddModelError("ManufacturerId", "Manufacturer is not found!");
                return View();
            }

            existProduct.ProductTags.RemoveAll(pt => !product.TagIds.Any(pId => pId == pt.TagId));

            foreach (var id in product.TagIds.Where(pt => !existProduct.ProductTags.Any(pId => pt == pId.TagId)))
            {
                ProductTag productTag = new ProductTag()
                {
                    TagId = id,
                };

                existProduct.ProductTags.Add(productTag);
            }



            existProduct.Name = product.Name;
            existProduct.Desc = product.Desc;
            existProduct.Tax = product.Tax;
            existProduct.Code = product.Code;
            existProduct.SalePrice = product.SalePrice;
            existProduct.CostPrice = product.CostPrice;
            existProduct.IsAvailable = product.IsAvailable;
            existProduct.DiscountPercent = product.DiscountPercent;
            existProduct.ManufacturerId = product.ManufacturerId;

            _DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
