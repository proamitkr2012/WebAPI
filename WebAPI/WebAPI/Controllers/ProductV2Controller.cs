using EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/product/v2/[action]")]
    [ApiController]
    public class ProductV2Controller : ControllerBase
    {
        private DatabaseContext _db;
        public ProductV2Controller(DatabaseContext db)
        {
            _db = db;
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            try
            {
                //TODO:
                var data = _db.Products.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
