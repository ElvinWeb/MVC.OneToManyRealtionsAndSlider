using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.SliderFrontToBack.Helpers;
using MVC.SliderFrontToBack.Models;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly ProjectDbContext _DbContext;
        private readonly IWebHostEnvironment _env;

        public ProductController(ProjectDbContext context, IWebHostEnvironment env)
        {
            _DbContext = context;
            _env = env;

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


            if (product.ProductMainImage != null)
            {

                if (product.ProductMainImage.ContentType != "image/png" && product.ProductMainImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProductMainImage", "please select correct file type");
                    return View();
                }

                if (product.ProductMainImage.Length > 1048576)
                {
                    ModelState.AddModelError("ProductMainImage", "file size should be more lower than 1mb ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "assets/books", product.ProductMainImage);
                ProductImage productImage = new ProductImage
                {
                    Product = product,
                    ImgUrl = newFileName,
                    isPoster = true,
                };
                _DbContext.ProductImages.Add(productImage);
            };

            if (product.ProductHoverImage != null)
            {

                if (product.ProductHoverImage.ContentType != "image/png" && product.ProductHoverImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProductHoverImage", "please select correct file type");
                    return View();
                }

                if (product.ProductHoverImage.Length > 1048576)
                {
                    ModelState.AddModelError("ProductHoverImage", "file size should be more lower than 1mb ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "assets/books", product.ProductHoverImage);
                ProductImage productImage = new ProductImage
                {
                    Product = product,
                    ImgUrl = newFileName,
                    isPoster = false,
                };
                _DbContext.ProductImages.Add(productImage);
            };

            if (product.ImageFiles != null)
            {
                foreach (var img in product.ImageFiles)
                {

                    if (img.ContentType != "image/png" && img.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "please select correct file type");
                        return View();
                    }

                    if (img.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFiles", "file size should be more lower than 1mb ");
                        return View();
                    }

                    string newFileName = Helper.GetFileName(_env.WebRootPath, "assets/books", img);
                    ProductImage productImage = new ProductImage
                    {
                        Product = product,
                        ImgUrl = newFileName,
                        isPoster = null,
                    };
                    _DbContext.ProductImages.Add(productImage);
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

            Product product = _DbContext.Products.Include(x => x.ProductImages).FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();


            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            Product wantedProduct = _DbContext.Products.Include(x => x.ProductImages).FirstOrDefault(p => p.Id == product.Id);

            if (wantedProduct == null) return NotFound();

            if (wantedProduct.ProductImages != null)
            {
                foreach (var image in wantedProduct.ProductImages)
                {
                    string folderPath = "assets/books";

                    if (image.isPoster == null)
                    {
                        string path = Path.Combine(_env.WebRootPath, folderPath, image.ImgUrl);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    if (image.isPoster == false)
                    {
                        string path = Path.Combine(_env.WebRootPath, folderPath, image.ImgUrl);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    if (image.isPoster == true)
                    {
                        string path = Path.Combine(_env.WebRootPath, folderPath, image.ImgUrl);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
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

            Product product = _DbContext.Products.Include(pt => pt.ProductTags).Include(x => x.ProductImages).FirstOrDefault(b => b.Id == id);

            if (product == null) return NotFound();

            product.TagIds = product.ProductTags.Select(t => t.TagId).ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {

            ViewBag.Manufacturers = _DbContext.Manufacturers.ToList();
            ViewBag.Tags = _DbContext.Tags.ToList();

            //if (!ModelState.IsValid) return View();

            Product existProduct = _DbContext.Products.Include(pt => pt.ProductTags).Include(x => x.ProductImages).FirstOrDefault(b => b.Id == product.Id);

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


            if (product.ProductMainImage != null)
            {
                existProduct.ProductImages.RemoveAll(pi => !product.ProductImageIds.Contains(pi.Id) && pi.isPoster == true);

                if (product.ProductMainImage.ContentType != "image/png" && product.ProductMainImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProductMainImage", "please select correct file type");
                    return View();
                }

                if (product.ProductMainImage.Length > 1048576)
                {
                    ModelState.AddModelError("ProductMainImage", "file size should be more lower than 1mb ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "assets/books", product.ProductMainImage);
                ProductImage productImage = new ProductImage
                {
                    Product = product,
                    ImgUrl = newFileName,
                    isPoster = true,
                };
                existProduct.ProductImages.Add(productImage);
            };

            if (product.ProductHoverImage != null)
            {
                existProduct.ProductImages.RemoveAll(pi => !product.ProductImageIds.Contains(pi.Id) && pi.isPoster == false);

                if (product.ProductHoverImage.ContentType != "image/png" && product.ProductHoverImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProductHoverImage", "please select correct file type");
                    return View();
                }

                if (product.ProductHoverImage.Length > 1048576)
                {
                    ModelState.AddModelError("ProductHoverImage", "file size should be more lower than 1mb ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "assets/books", product.ProductHoverImage);
                ProductImage productImage = new ProductImage
                {
                    Product = product,
                    ImgUrl = newFileName,
                    isPoster = false,
                };
                existProduct.ProductImages.Add(productImage);
            };

            existProduct.ProductImages.RemoveAll(pi => !product.ProductImageIds.Contains(pi.Id) && pi.isPoster == null);
            if (product.ImageFiles != null)
            {
                foreach (var img in product.ImageFiles)
                {

                    if (img.ContentType != "image/png" && img.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "please select correct file type");
                        return View();
                    }

                    if (img.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFiles", "file size should be more lower than 1mb ");
                        return View();
                    }

                    string newFileName = Helper.GetFileName(_env.WebRootPath, "assets/books", img);
                    ProductImage productImage = new ProductImage
                    {
                        Product = product,
                        ImgUrl = newFileName,
                        isPoster = null,
                    };
                    existProduct.ProductImages.Add(productImage);
                }
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


