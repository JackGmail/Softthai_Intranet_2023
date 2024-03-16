using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models;

namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailRequestController : ControllerBase
    {
        private readonly IEmailRequestService _service;
        public EmailRequestController(IEmailRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetEmail(EmailRequest objEmail)
        {
            var result = _service.GetEmail(objEmail);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEmailConfirmation(EmailData objEmailConfirm)
        {
            var result = _service.SaveEmailConfirmation(objEmailConfirm);
            return StatusCode(result.nStatusCode, result);
        }
    }
}