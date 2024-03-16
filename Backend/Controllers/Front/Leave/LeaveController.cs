using Backend.Models;
using Extensions.Common.STResultAPI;
using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models;
using ST_API.Services.ISystemService;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;

namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _service;

        public LeaveController(ILeaveService service)
        {
            this._service = service;
        }
      
        [HttpGet]
        public async Task<IActionResult> GetFormRequest(string? sID)
        {
            cFormInitRequest result = await _service.GetFormRequest(sID);
            return StatusCode(result.Status, result);
        }

        [HttpPost]

        public async Task<IActionResult> ApproveOnTable(cFormReqRequest req)
        {
            ResultAPI result = await _service.ApproveOnTable(req);
            return StatusCode(result.Status, result);
        }

        

        [HttpGet]
        public async Task<IActionResult> GetFilterLeave()
        {
            cFormInitRequest result = await _service.GetFilterLeave();
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLog(string sID)
        {
            cLeaveLogsRequest result = await _service.GetLog(sID);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(cFormReqRequest req)
        {
            ResultAPI result = await _service.SaveData(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetMasterData()
        {
            ResultAPI result = this._service.GetMasterData();
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult LeaveImportExcel(cLeaveImportExcelRequest req)
        {
            var result = this._service.LeaveImportExcel(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult LeaveExportExcel(cLeaveExportRequest req)
        {
            var result = this._service.LeaveExportExcel(req);
            return File(result.objFile, result.sFileType, result.sFileName);
        }

        [HttpPost]
        public IActionResult EditLeaveSummary(cEditLeaveRequest req)
        {
            ResultAPI result = this._service.EditLeaveSummary(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataWorkList(cReqTableWorkListRequest req)
        {
            cTabelWorkListRequest result = await this._service.GetDataWorkList(req);
            return StatusCode(result.Status, result);
        }
         [HttpPost]
        public IActionResult RemoveDataTable(cRemoveTable param)
        {
            var result = _service.RemoveDataTable(param);
            return StatusCode(result.Status, result);
        }
    }
}