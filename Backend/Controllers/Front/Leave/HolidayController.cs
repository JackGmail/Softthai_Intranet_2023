using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models.IHolidayService;

namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _service;
        public HolidayController(IHolidayService service)
        {
            this._service = service;
        }

        /// <summary>
        /// บันทึกข้อมูลวันหยุดประจำปี
        /// </summary>
        [HttpPost]
        public IActionResult SaveDataHoliday(cHolidayYear param)
        {
            var result = this._service.SaveDataHoliday(param);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// ดึงข้อมูลวันหยุดประจำปี
        /// </summary>
        [HttpPost]
        public IActionResult GetHolidayList(cHolidayData cResult)
        {
            var result = this._service.GetHolidayList(cResult);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// ดึงข้อมูลของแต่ละปีที่เก็บวันหยุด
        /// </summary>
        [HttpPost]
        public IActionResult GetHolidayYearList(cReqHolidayYear cResult)
        {
            var result = this._service.GetHolidayYearList(cResult);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// ดึงข้อมูล options ปีตั้งต้น
        /// </summary>
        [HttpPost]
        public IActionResult GetYearOptionList()
        {
            var result = this._service.GetYearOptionList();
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// Dupliacate วันหยุดทั้งหมดมาจากปีที่ตั้งต้น
        /// </summary>
        [HttpPost]
        public IActionResult DuplicateHolidayList(cYearList req)
        {
            var result = this._service.DuplicateHolidayList(req);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// สร้าง PDF Document รายการวันหยุดของปีที่เลือก
        /// </summary>
        #region Export PDF
        [HttpPost]
        public async Task<FileResult> ExportPDFHoliday(cHolidayData param)
        {
            var result = await _service.ExportPDFHoliday(param);
            // Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            return File(result.objFile ?? new byte[] { }, result.sFileType ?? ".pdf", result.sFileName);
            // return File(result.objFile, result.sFileType, result.sFileName);
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> RemoveDataTable(cRemoveTableHoliday param)
        {
            var result = await this._service.RemoveDataTable(param);
            return StatusCode(result.Status, result);
        }
    }
}