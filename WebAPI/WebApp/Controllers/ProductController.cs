using EFCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        DatabaseContext _db;
        public ProductController(DatabaseContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //var products = _db.Products.ToList();
            //var products = _db.Products.Where(p => p.ProductId > 0).ToList();

            // OR
            var products = (from prd in _db.Products
                            where prd.ProductId > 0
                            select prd).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            try
            {
                _db.Products.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

            }
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        public IActionResult Edit(int id)
        {
            //Product product = _db.Products.Find(id);
            Product product = _db.usp_getproduct(id);

            ViewBag.Categories = _db.Categories.ToList();
            return View("Create",product);
        }

        [HttpPost]
        public IActionResult Edit(Product model)
        {
            try
            {
                _db.Products.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

            }
            ViewBag.Categories = _db.Categories.ToList();
            return View("Create", model);
        }

        public IActionResult Delete(int id)
        {
            Product product = _db.Products.Find(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
