using EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private DatabaseContext _db;
        public ProductController(DatabaseContext db)
        {
            _db = db;
        }

        //GET:api/product
        //[HttpGet]
        //public IEnumerable<Product> GetAll()
        //{
        //    return _db.Products.ToList(); //200OK
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var data = _db.Products.ToList();
        //    return Ok(data);
        //    //return StatusCode(StatusCodes.Status202Accepted, data);
        //}

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("GetProducts", "Retrieve Product List")]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll(int version=1)
        {
            try
            {
                if (version == 2)
                {
                    var data = _db.Products.ToList();
                    return Ok(data);
                }
                else
                {
                    var data = _db.Products.ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            //return StatusCode(StatusCodes.Status202Accepted, data);
        }

        //GET:api/product/{12}
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _db.Products.Find(id);
        }

        [HttpPost]
        public IActionResult Add(Product model)
        {
            try
            {
                _db.Products.Add(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody]Product model)  //parameter binding
        {
            try
            {
                if (id != model.ProductId)
                    return BadRequest();

                _db.Products.Update(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch]
        public IActionResult Modify(Product model)
        {
            try
            {
                Product data = _db.Products.Find(model.ProductId);
                data.Name = model.Name;
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product model = _db.Products.Find(id);
            if (model != null)
            {
                _db.Products.Remove(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
