using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models.ITypeleaveService;

namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TypeleaveController : ControllerBase
    {
        private readonly ITypeleaveService _TypeleaveService;
        public TypeleaveController(ITypeleaveService TypeleaveService)
        {
            this._TypeleaveService = TypeleaveService;
        }
        [HttpGet]
        public async Task<IActionResult> GetData_MasterDetail()
        {
            var result = await this._TypeleaveService.GetData_MasterDetail();
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveLeaveType(TypeleaveData objSave)
        {
            var result = await this._TypeleaveService.SaveLeaveType(objSave);
            return StatusCode(result.Status, result);
        }


        [HttpPost]
        public async Task<IActionResult> GetTableLeaveType(cReqTableTypeleave param)
        {
            var result = await this._TypeleaveService.GetTableLeaveType(param);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataEdit(cParam param)
        {
            var result = await this._TypeleaveService.GetDataEdit(param);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveDataTable(cRemoveTableLeave param)
        {
            var result = await this._TypeleaveService.RemoveDataTable(param);
            return StatusCode(result.Status, result);
        }



        [HttpPost]
        public async Task<IActionResult> GetTableLeaveSummary(cReqTableLeaveSummary param)
        {
            var result = await this._TypeleaveService.GetTableLeaveSummary(param);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveDataTableSummary(cRemoveTableLeave param)
        {
            var result = await this._TypeleaveService.RemoveDataTableSummary(param);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTypeSummary(cParamSummary param)
        {
            var result = await this._TypeleaveService.GetDataTypeSummary(param);
            return StatusCode(result.Status, result);
        }


        [HttpPost]
        public async Task<IActionResult> SaveLeaveSummary(cSaveSummary objSave)
        {
            var result = await this._TypeleaveService.SaveLeaveSummary(objSave);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<FileResult> ExportExcelSummary(cParamExport param)
        {
            var result = await this._TypeleaveService.ExportExcelSummary(param);
            return File(result.objFile, result.sFileType, result.sFileName);
        }

        [HttpPost]
        public async Task<IActionResult> CheckAlreadyStart(ParamCheck param)
        {
            var result = await this._TypeleaveService.CheckAlreadyStart(param);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public async Task<IActionResult> CheckAlreadyEnd(ParamCheck param)
        {
            var result = await this._TypeleaveService.CheckAlreadyEnd(param);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActive(cTableTypeleave param)
        {
            var result = await this._TypeleaveService.ToggleActive(param);
            return StatusCode(result.Status, result);
        }


        [HttpPost]
        public async Task<IActionResult> GetData_Dashboard(cParamSearch Param)
        {
            var result = await this._TypeleaveService.GetData_Dashboard(Param);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public async Task<IActionResult> SyncDataTypeleave(cSysTypeleave Param)
        {
            var result = await this._TypeleaveService.SyncDataTypeleave(Param);
            return StatusCode(result.Status, result);
        }
    }

}