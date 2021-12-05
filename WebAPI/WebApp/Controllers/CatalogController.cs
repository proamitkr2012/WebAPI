using EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class CatalogController : Controller
    {
        HttpClient client;
        Uri baseAddress;
        IConfiguration config;
        public CatalogController(IConfiguration _config)
        {
            client = new HttpClient();
            config = _config;
            baseAddress = new Uri(config["ApiAddress"]);
            client.BaseAddress = baseAddress;
        }

        private IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> model = new List<Category>();
            var response = client.GetAsync(client.BaseAddress + "/category/getall").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<Category>>(data);
            }
            return model;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<Product> model = new List<Product>();
        //    var response = client.GetAsync(client.BaseAddress + "/product/getall").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = response.Content.ReadAsStringAsync().Result;
        //        model = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
        //    }
        //    return View(model);
        //}

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> model = new List<Product>();
            var response = await client.GetAsync(client.BaseAddress + "/product/getall");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                model = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
            }
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            ModelState.Remove("ProductId");
            if (ModelState.IsValid)
            {
                string strData = JsonSerializer.Serialize(model);
                StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress + "/product/add", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = GetCategories();
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = GetCategories();
            Product model = new Product();
            var response = client.GetAsync(client.BaseAddress + "/product/get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<Product>(data);
            }

            return View("Create",model);
        }

        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                string strData = JsonSerializer.Serialize(model);
                StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
                var response = client.PutAsync(client.BaseAddress + "/product/update/" + model.ProductId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = GetCategories();
            return View("Create");
        }

        public IActionResult Delete(int id)
        {
            var response = client.DeleteAsync(client.BaseAddress + "/product/delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
