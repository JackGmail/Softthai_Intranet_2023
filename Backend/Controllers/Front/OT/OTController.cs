using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class OTController : ControllerBase
    {
        private readonly IOTService _service;
        public OTController(IOTService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get ข้อมูลตั้งต้นของ Form
        /// </summary>
        [HttpGet]
        public IActionResult LoadDataInit()
        {
            var result = _service.LoadDataInit();
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get Project Task
        /// </summary>
        [HttpPost]
        public IActionResult GetProjectTask(cReqDataOT param)
        {
            var result = _service.GetProjectTask(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get Reason
        /// </summary>
        [HttpPost]
        public IActionResult GetReason(cReqDataOT param)
        {
            var result = _service.GetReason(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Save ข้อมูลของ Form
        /// </summary>
        [HttpPost]
        public IActionResult SaveData(cOTData param)
        {
            var result = _service.SaveData(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Save ข้อมูลผลการทำ OT
        /// </summary>
        [HttpPost]
        public IActionResult SaveResult(cOTData param)
        {
            var result = _service.SaveResult(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลเริ่มต้นของ List
        /// </summary>
        [HttpGet]
        public IActionResult GetInitDataTable()
        {
            var result = _service.GetInitDataTable();
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลรายการ OT
        /// </summary>
        [HttpPost]
        public IActionResult GetDataTable(cReqTableOT param)
        {
            var result = _service.GetDataTable(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลของ Form
        /// </summary>
        [HttpPost]
        public IActionResult GetDataOT(cReqDataOT param)
        {
            var result = _service.GetDataOT(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// ลบ Row ใน Table List
        /// </summary>
        [HttpPost]
        public IActionResult RemoveDataTable(cRemoveTable param)
        {
            var result = _service.RemoveDataTable(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Reject OT
        /// </summary>
        [HttpPost]
        public IActionResult Reject(cReqDataOT param)
        {
            var result = _service.Reject(param);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// Reprot OT
        /// </summary>
        [HttpPost]
        public async Task<FileResult> ExportExcelOT(cReqTableOT param)
        {
            var result = await this._service.ExportExcelOT(param);
            return File(result.objFile, result.sFileType, result.sFileName);
        }
    }

}
