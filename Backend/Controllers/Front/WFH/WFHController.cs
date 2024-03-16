using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Back.Permission;

namespace Backend.Controllers
{
    /// <summary>
    /// </summary>

    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class WFHController : ControllerBase
    {
        private readonly IWFHService _service;

        public WFHController(IWFHService service)
        {
            this._service = service;
        }

        [HttpPost]
        public IActionResult UpdateFlowWorkFromHome(cUpdateFlow param)
        {
            var result = _service.UpdateFlowWorkFromHome(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult OptionApproverWFH(ParamProjectID param)
        {
            var result = _service.OptionApproverWFH(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult GetActionFlowWorkFromHome(string? Key)
        {
            var result = _service.GetActionFlowWorkFromHome(Key);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult WFHWorkflow(cWFHFlow oWFH)
        {
            var result = this._service.WFHWorkflow(oWFH);
            return StatusCode(200, result);
        }
    
        [HttpGet]
        public IActionResult TestSendMailWFH(int nWFHID, int nFlowProcessID)
        {
            var result = this._service.SendMailWFH(nWFHID, nFlowProcessID);
            return StatusCode(200, result);
        }

        [HttpGet]
        public IActionResult GetDataTable([FromQuery] clcFilterWFH oWFH)
        {

            var result = this._service.GetDataTable(oWFH);
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public IActionResult GetOption()
        {

            var result = this._service.GetOption();
            return StatusCode(result.Status, result);
        }
        [HttpGet]
        public IActionResult GetInitData()
        {
            var result = this._service.GetInitData();
            return StatusCode(result.Status, result);
        }
     
        [HttpPost]
        public IActionResult GetDataTableTaskPlan(cTaskTable req)
        {
            var result = this._service.GetDataTableTaskPlan(req);
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public IActionResult GetFormWFH(string sWFHID)
        {
            var result = this._service.GetFormWFH(sWFHID);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpGet]
        public IActionResult GetHistoryWFH(string? sWFHID)
        {
            var result = this._service.GetHistoryWFH(sWFHID);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveData(cWFHSave param)
        {
            var result = this._service.SaveData(param);
            return StatusCode(result.nStatusCode, result);
        }

         [HttpPost]
        public IActionResult DeleteData(cReqDeleteUser param)
        {
            var result = this._service.DeleteData(param);
            return StatusCode(result.nStatusCode, result);
        }
    }
}

