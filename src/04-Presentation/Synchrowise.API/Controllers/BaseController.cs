using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Synchrowise.Contract.Response;

namespace Synchrowise.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(ApiResponse<T> response) where T: class
        {
            return new ObjectResult(response){
                StatusCode = response.StatusCode
            };
        }
    }
}