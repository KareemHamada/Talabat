﻿namespace Talabat.Apis.Controllers
{

    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {

        public ActionResult Error(int code)
        {
            return NotFound(new APIResponse(code)); 
        }
    }
}
