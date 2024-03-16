using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Extensions.Common.STResultAPI;


namespace Backend.Controllers.ApproveLine
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApproveLineController : ControllerBase
    {
        private readonly IApproveLineService _service;
        public ApproveLineController(IApproveLineService service)
        {
            this._service = service;
        }

        [HttpGet]
        public IActionResult GetDataMenuLine()
        {
            var result = this._service.GetDataMenuLine();
            return StatusCode(result.Status, result);
        }
    }
}
