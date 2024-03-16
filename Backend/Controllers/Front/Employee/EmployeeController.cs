using Microsoft.AspNetCore.Mvc;
using Backend.EF.ST_Intranet;
using Backend.Models;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ST_IntranetEntity _db;
        public EmployeeController(IEmployeeService service, ST_IntranetEntity db)
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
        #region Sun
        [HttpGet]
        public IActionResult GetInitEmployeeList()
        {
            var result = this._service.GetInitEmployeeList();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetDataEmployee(cReqEmployee param)
        {
            return Ok(this._service.GetDataEmployee(param));
        }
        [HttpGet]
        public IActionResult GetDataEmployeeForm(string? sEmployeeID)
        {
            var result = this._service.GetDataEmployeeForm(sEmployeeID);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveDataEmployee(cEmployeeForm param)
        {
            var result = this._service.SaveDataEmployee(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpGet]
        public IActionResult GetDataFamilyForm(string? sEmployeeID)
        {
            var result = this._service.GetDataFamilyForm(sEmployeeID);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveDataFamily(cFamily param)
        {
            var result = this._service.SaveDataFamily(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpGet]
        public IActionResult GetDataLanguageForm(string? sEmployeeID)
        {
            var result = this._service.GetDataLanguageForm(sEmployeeID);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveDataLanguage(cLanguage req)
        {
            var result = this._service.SaveDataLanguage(req);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpGet]
        public IActionResult GetDataPositionForm(string? sEmployeeID)
        {
            var result = this._service.GetDataPositionForm(sEmployeeID);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveDataPosition(cPosition param)
        {
            var result = this._service.SaveDataPosition(param);
            return StatusCode(result.nStatusCode, result);
        }

        #endregion

        #region Or
        [HttpPost]
        public IActionResult SaveDataEducation(cEducation req)
        {
            var result = this._service.SaveDataEducation(req);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult SaveDataWorkExperien(cWorkExperien req)
        {
            var result = this._service.SaveDataWorkExperien(req);
            return StatusCode(result.nStatusCode, result);
        }



        [HttpPost]
        public IActionResult GetDataEducation(cEducation req)
        {
            var result = this._service.GetDataEducation(req);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult GetDataWorkExperien(cWorkExperien req)
        {
            var result = this._service.GetDataWorkExperien(req);
            return StatusCode(result.nStatusCode, result);
        }


        [HttpPost]
        public IActionResult SaveDataOtherParts(cOtherParts req)
        {
            var result = this._service.SaveDataOtherParts(req);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult GetDataOtherParts(cOtherParts req)
        {
            var result = this._service.GetDataOtherParts(req);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveDataSpecialAbility(cSpecialAbility req)
        {
            var result = this._service.SaveDataSpecialAbility(req);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult GetDataSpecialAbility(cSpecialAbility req)
        {
            var result = this._service.GetDataSpecialAbility(req);
            return StatusCode(result.nStatusCode, result);
        }

        #endregion

        #region ตี้ GetProfile
        [HttpGet]
        public IActionResult GetProfile(string? sID)
        {
            var result = this._service.GetProfile(sID);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public FileResult SpireDoc_ExportWord(objParam obj)
        {
            cFileZip objFile = new cFileZip();
            var result = this._service.SpireDoc_ExportWord(obj);
            cReturnExport objImport = result;
            return File(objImport.objFile, objImport.sFileType, objImport.sFileName);
        }
        #endregion

        [HttpGet]
        public IActionResult GetEmployeeID()
        {
            var result = this._service.GetEmployeeID();
            return StatusCode(result.nStatusCode, result);
        }
        [HttpGet]
        public IActionResult GetMenueEmployee()
        {
            var result = this._service.GetMenueEmployee();
            return StatusCode(result.nStatusCode, result);
        }
    }
}

