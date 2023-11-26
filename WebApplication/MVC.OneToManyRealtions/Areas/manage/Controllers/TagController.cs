﻿using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.SliderFrontToBack.Models;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        private readonly ProjectDbContext _DbContext;

        public TagController(ProjectDbContext context)
        {
            _DbContext = context;

        }
        public IActionResult Index()
        {
            List<Tag> tags = _DbContext.Tags.ToList();

            return View(tags);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid) return View();

            if (_DbContext.Tags.Any(t => t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "tag has already created!");
                return View();
            }

            _DbContext.Tags.Add(tag);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();

            Tag tag = _DbContext.Tags.FirstOrDefault(t => t.Id == id);

            if (tag == null) return NotFound();

            return View(tag);
        }

        [HttpPost]
        public IActionResult Update(Tag tag)
        {
            if (!ModelState.IsValid) return View();

            Tag existTag = _DbContext.Tags.FirstOrDefault(t => t.Id == tag.Id);
            if (existTag == null) return NotFound();

            if (_DbContext.Tags.Any(t => t.Id != tag.Id && t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "tag has already created!");
                return View();
            }

            existTag.Name = tag.Name;

            _DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Tag tag = _DbContext.Tags.FirstOrDefault(t => t.Id == id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(Tag tag)
        {

            Tag existTag = _DbContext.Tags.FirstOrDefault(t => t.Id == tag.Id);

            if (existTag == null)
            {
                return NotFound();
            }

            _DbContext.Tags.Remove(existTag);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
