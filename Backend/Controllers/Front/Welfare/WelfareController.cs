using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WelfareController : ControllerBase
    {
        private readonly IWelfareService _service;

        public WelfareController(IWelfareService service)
        {
            this._service = service;
        }

        [HttpGet]
        public IActionResult SelectDentalType()
        {
            var result = this._service.SelectDentalType();
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult GetData_Dental(cDentalTable obj)
        {
            var result = this._service.GetData_Dental(obj);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult SaveData_DentalRequest(cDentalDetail objSave)
        {
            var result = this._service.SaveData_DentalRequest(objSave);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult SaveApprove(cReqWelfareApprove param)
        {
            var result = this._service.SaveApprove(param);
            return StatusCode(result.nStatusCode, result);
        }
    }
}