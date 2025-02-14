using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Errors;
using Talabat.Repository.Data;

namespace Talabat.Apis.Controllers
{

    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("NotFound")]
        public IActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if(product == null) 
                return NotFound( new APIResponse(404));
            
            return Ok(product);
        }

        [HttpGet("ServerError")]
        public IActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product.ToString();
            // will throw exception [null reference exception]
            return Ok(productToReturn);
        }

        [HttpGet("BadRequest")]
        public IActionResult GetBadRequest()
        {
            
            return BadRequest();
        }

        // validation error
        [HttpGet("BadRequest/{id}")]
        public IActionResult GetBadRequest(int id)
        {

            return Ok();
        }
    }
}
