
using Microsoft.AspNetCore.Mvc;
using Backend.EF.ST_Intranet;
using Backend.Models;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DailyTaskController : ControllerBase
    {
        private readonly IDailyTaskService _service;
        public DailyTaskController(IDailyTaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult PageLoad(bool IsFilterUserData)
        {
            var result = _service.PageLoad(IsFilterUserData);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetTask(ParamTask param)
        {
            var result = _service.GetTask(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult SaveTask(ParamSaveTask param)
        {
            var result = _service.SaveTask(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult AddTask(ParamAddTask param)
        {
            var result = _service.AddTask(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult RemoveTask(ParamRemoveTask param)
        {
            var result = _service.RemoveTask(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetTaskOverAll(ParamTaskOverAll param)
        {
            var result = _service.GetTaskOverAll(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetTeamEmployee([FromBody] ParamTeamEmployee param)
        {
            var result = _service.GetTeamEmployee(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult GetTaskRule()
        {
            var result = _service.GetTaskRule();
            return StatusCode(result.nStatusCode, result);
        }

        #region TaskFormList
        [HttpPost]
        public IActionResult GetTaskFormList([FromBody] ParamSearchTask param)
        {
            var result = _service.GetTaskFormList(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveTaskFormList([FromBody] ParamSaveTaskFormList param)
        {
            var result = _service.SaveTaskFormList(param);
            return StatusCode(result.nStatusCode, result);
        }
        #endregion

        #region TaskPlanMultiDate
        [HttpPost]
        public IActionResult SaveTaskPlanMultiDate([FromBody] ParamSaveTaskMultiDate param)
        {
            var result = _service.SaveTaskPlanMultiDate(param);
            return StatusCode(result.nStatusCode, result);
        }
        #endregion

        [HttpPost]
        public IActionResult onExportTaskMonitorReport(ExportTaskMonitorReport param)
        {
            var result = _service.onExportTaskMonitorReport(param);
            return File(result.objFile, result.sFileType, result.sFileName);
        }
    }
}