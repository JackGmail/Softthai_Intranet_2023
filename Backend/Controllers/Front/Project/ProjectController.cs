using Backend.Interfaces.Front.Project;
using Backend.Models.Front.Project;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Backend.Controllers.Front.Project
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service)
        {
            this._service = service;
        }

        [HttpGet]
        public IActionResult GetInitData()
        {
            var result = this._service.GetInitData();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult SaveData(cProject req)
        {
            var result = this._service.SaveData(req);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult GetDataInfo(int nContractPointID)
        {
            var result = this._service.GetDataInfo(nContractPointID);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult GetSearchEmployee(string strSearch)
        {
            return Ok(this._service.GetSearchEmployee(strSearch));
        }

        [HttpGet]
        public IActionResult GetData(string sID)
        {
            var result = this._service.GetData(sID);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult GetInitDataProcess()
        {
            var result = this._service.GetInitDataProcess();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult SaveDataProcess(cProcess req)
        {
            var result = this._service.SaveDataProcess(req);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult GetDataProcess(int nProcessID)
        {
            var result = this._service.GetDataProcess(nProcessID);
            return StatusCode(result.nStatusCode, result);
        }

        //[HttpPost]
        //public IActionResult GetDataTable(cProcessTable param)
        //{
        //    var result = this._service.GetDataTable(param);
        //    return StatusCode(result.nStatusCode, result);
        //}

        [HttpPost]
        public IActionResult ChangeOrder(cProcess param)
        {
            var result = this._service.ChangeOrder(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult GetDataTableProject(cProjectTable req)
        {
            var result = this._service.GetDataTableProject(req);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult RemoveDataTable(cProjectTable req)
        {
            var result = this._service.RemoveDataTable(req);
            return StatusCode(result.nStatusCode, result);
        }
        #region sun
        [HttpGet]
        public IActionResult GetDataProjectTOR(string? sProjectID)
        {
            var resutl = this._service.GetDataProjectTOR(sProjectID);
            return StatusCode(resutl.nStatusCode, resutl);
        }
        [HttpPost]
        public IActionResult SaveProjectTOR(cProjectTOR req)
        {
            var result = this._service.SaveProjectTOR(req);
            return StatusCode(result.nStatusCode, result);
        }
        //[HttpPost]
        //public FileResult ExportExcelTOR(cExportExcelTOR param)
        //{
        //    var objData = this._service.ExportExcelTOR(param);
        //    return File(objData.objFile, objData.sFileType, objData.sFileName);
        //}
        //[HttpPost]
        //public IActionResult ImportExcelTOR(reqImportTOR req)
        //{
        //    var result = this._service.ImportExcelTOR(req);
        //    return StatusCode(result.nStatusCode, result);
        //}
        #endregion

        ///// <summary>
        ///// </summary>
        //[HttpGet]
        //public IActionResult OptionPlanTest()
        //{
        //    var result = this._service.OptionPlanTest();
        //    return StatusCode(result.Status, result);
        //}

        ///// <summary>
        ///// </summary>
        //[HttpGet]
        //public IActionResult GetDataTablePlanTest([FromQuery] clsFilterProject obj)
        //{
        //    var result = this._service.GetDataTablePlanTest(obj);
        //    return StatusCode(result.Status, result);
        //}
    }
}
