using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace ST_API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class TravelController : ControllerBase
    {
        private readonly ITravelService _service;
        public TravelController(ITravelService service)
        {
            this._service = service;
        }
        /// <summary>
        /// Save ข้อมูลของ Form Travel
        /// </summary>
        [HttpPost]
        public IActionResult SaveData(cTravelData param)
        {
            var result = this._service.SaveData(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลของ Form
        /// </summary>
        [HttpPost]
        public IActionResult GetDataTravel(cReqDataAllowance param)
        {
            var result = this._service.GetDataTravel(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Approve, Reject, Cancel
        /// </summary>
        [HttpPost]
        public IActionResult SaveApprove(cReqApprove param)
        {
            var result = this._service.SaveApprove(param);
            return StatusCode(result.Status, result);
        }
    }

}
