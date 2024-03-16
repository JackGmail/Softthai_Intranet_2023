using Backend.Models;
using Extensions.Common.STResultAPI;
using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models;


namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterProcessController : ControllerBase
    {
        private readonly IMasterProcessService _service;
        public MasterProcessController(IMasterProcessService service)
        {
            this._service = service;
        }

        [HttpPost]
        public IActionResult LoadMainProcessOptions()
        {
            var result = this._service.LoadMainProcessOptions();
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult SaveProcessData(cReqMasterProcessData req)
        {
            var result = this._service.SaveProcessData(req);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult LoadMasterProcessData(cMasterProcessData req)
        {
            var result = this._service.LoadMasterProcessData(req);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult LoadSubProcessData(cMasterProcessData req)
        {
            var result = this._service.LoadSubProcessData(req);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult ChangeOrder(cMasterProcessOrder req)
        {
            var result = this._service.ChangeOrder(req);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult GetDataToEdit(cReqMasterProcessDataValue req)
        {
            var result = this._service.GetDataToEdit(req);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveProcessData(cRemoveTableMaster req)
        {
            var result = await this._service.RemoveProcessData(req);
            return StatusCode(result.Status, result);
        }

    }
}