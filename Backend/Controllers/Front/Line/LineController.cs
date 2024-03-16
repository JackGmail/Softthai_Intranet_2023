using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models;

namespace ST_API.Controllers
{
     [Route("api/[controller]/[action]")]
    [ApiController]
    public class LineController : ControllerBase
    {
                private readonly ILineService _service;
        public LineController(ILineService service)
        {
            this._service = service;
        }
            [HttpGet]
        public IActionResult TestSend()
        {
           
            var result = this._service.TestSend();
            return StatusCode(200, result);
        }
             [HttpPost]
        public IActionResult LineWebhook(dynamic request)
        {
               this._service.Webhook(request);
            return Ok();
        }
    }
}