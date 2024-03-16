using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace ST_API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AllowanceController : ControllerBase
    {
        private readonly IAllowanceService _service;
        public AllowanceController(IAllowanceService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Get ข้อมูลตั้งต้นของ Form
        /// </summary>
        [HttpGet]
        public IActionResult LoadDataInit()
        {
            var result = this._service.LoadDataInit();
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Save ข้อมูลของ Form Allowance
        /// </summary>
        [HttpPost]
        public IActionResult SaveData(cAllowanceData param)
        {
            var result = this._service.SaveData(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลเริ่มต้นของ List
        /// </summary>
        [HttpGet]
        public IActionResult GetInitDataTable()
        {
            var result = this._service.GetInitDataTable();
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลรายการ Allowance
        /// </summary>
        [HttpPost]
        public IActionResult GetDataTable(cReqTableWelfare param)
        {
            var result = this._service.GetDataTable(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// Get ข้อมูลของ Form
        /// </summary>
        [HttpPost]
        public IActionResult GetDataAllowance(cReqDataAllowance param)
        {
            var result = this._service.GetDataAllowance(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// ลบ Row ใน Table List
        /// </summary>
        [HttpPost]
        public IActionResult RemoveDataTable(cRemoveTable param)
        {
            var result = this._service.RemoveDataTable(param);
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
