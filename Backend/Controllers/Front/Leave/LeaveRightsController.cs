using Extensions.Common.STResultAPI;
using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models;

namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveRightsController : ControllerBase
    {
        private readonly ILeaveRightsService _service;

        public LeaveRightsController(ILeaveRightsService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> SumLeaveCard()
        {
            var result = await this._service.SumLeaveCard();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTableLeave(cLeaveTable req)
        {
            cResultTableLeave result = this._service.GetDataTableLeave(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTable(cLeaveTable req)
        {
            var result = new cResultTableLeave();
            if (req.nID == 2)
            {
                result = this._service.GetDataTableLate(req);
            }
            else if (req.nID == 3)
            {
                result = this._service.GetDataTableWFH(req);
            }
            else if (req.nID == 4)
            {
                result = this._service.GetDataTableWorkCert(req);
            }
            else if (req.nID == 5)
            {
                result = this._service.GetDataTableHoliday(req);
            }

            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOnTable(cFormReq req)
        {
            ResultAPI result = await _service.ApproveOnTable(req);
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilterLeave()
        {
            cFormInit result = await _service.GetFilterLeave();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetLeaveSummary(cLeaveRights req)
        {
            ResultAPI result = this._service.GetLeaveSummary(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetMasterData()
        {
            ResultAPI result = this._service.GetMasterData();
            return StatusCode(result.Status, result);
        }

        // [HttpPost]
        // // public IActionResult LeaveImportExcel(cLeaveImportExcel req)
        // public IActionResult LeaveImportExcel(reqImport req)
        // {
        //     var result = this._service.ImportExcelTOR(req);
        //     return StatusCode(result.nStatusCode, result);
        // }

        [HttpPost]
        public IActionResult LeaveExportExcel(cLeaveExport req)
        {
            var result = this._service.LeaveExportExcel(req);
            return File(result.objFile, result.sFileType, result.sFileName);
        }

        [HttpPost]
        public IActionResult EditLeaveSummary(cEditLeave req)
        {
            ResultAPI result = this._service.EditLeaveSummary(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataWorkList(cReqTableWorkList req)
        {
            cTabelWorkList result = await this._service.GetDataWorkList(req);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult ImportExcel(cImport objSaveData)
        {
            ResultAPI result = this._service.ImportExcel(objSaveData);
            return StatusCode(result.Status, result);
        }
    }
}