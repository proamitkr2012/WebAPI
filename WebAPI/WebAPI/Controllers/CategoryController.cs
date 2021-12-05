using EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private DatabaseContext _db;
        public CategoryController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            var data = _db.Categories.ToList();
            return Ok(data);
        }
    }
}