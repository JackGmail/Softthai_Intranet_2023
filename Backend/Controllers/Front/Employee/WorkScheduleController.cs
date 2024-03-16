using Microsoft.AspNetCore.Mvc;
using Backend.EF.ST_Intranet;
using Backend.Models;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleService _service;
        private readonly ST_IntranetEntity _db;
        public WorkScheduleController(IWorkScheduleService service, ST_IntranetEntity db)
        {
            _service = service;
            _db = db;
        }

        [HttpGet]
        public IActionResult GetInitData()
        {
            var result = this._service.GetInitData();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetDataTable(cGetFilter objFilter)
        {
            var result = this._service.GetDataTable(objFilter);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult GetEmployeeData(cEmployeeData param)
        {
            var result = this._service.GetEmployeeData(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetDataTableEdit(cGetFilter objFilter)
        {
            var result = this._service.GetDataTableEdit(objFilter);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult SaveEmployeeData(cSaveEmployeeData objSaveData)
        {
            var result = this._service.SaveEmployeeData(objSaveData);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult WorkShceduleExportExcel(cExportRequest objExport)
        {
            // var result = new cExport();
            var result = this._service.WorkShceduleExportExcel(objExport);
            return File(result.objFile, result.sFileType, result.sFileName);
        }

        [HttpPost]
        public IActionResult GetDataExportExcel(cGetDataForExcel objData)
        {
            var result = this._service.GetDataExportExcel(objData);
            return StatusCode(result.Status, result);
        }
    }
}