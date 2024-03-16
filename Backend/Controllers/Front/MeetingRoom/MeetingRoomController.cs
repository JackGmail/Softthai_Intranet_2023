using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class MeetingRoomController : ControllerBase
    {
        private readonly IMeetingRoomService _service;
        /// <summary>
        /// MeetingRoomController
        /// </summary>
        public MeetingRoomController(IMeetingRoomService service)
        {
            this._service = service;
        }

        /// <summary>
        /// GetListCalendar
        /// </summary>
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult GetListCalendar(clsFilterMeeting param)
        {
            var result = this._service.GetListCalendar(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// GetOption
        /// </summary>

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult GetOption(clsFilterCheckCo param)
        {
            var result = this._service.GetOption(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// SaveForm
        /// </summary>
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult SaveForm(clsSaveMeeting param)
        {
            var result = this._service.SaveForm(param);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// GetData
        /// </summary>
        [HttpGet]
        public IActionResult GetData(string nID, string? Mode)
        {
            var result = this._service.GetData(nID, Mode);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// GetListRoom
        /// </summary>
        [HttpGet]
        public IActionResult GetListRoom()
        {
            var result = this._service.GetListRoom();
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// GetPerson
        /// </summary>
        [HttpGet]
        public IActionResult GetPerson(int nProjectID)
        {
            var result = this._service.GetPerson(nProjectID);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// GetFileZip Zip file
        /// </summary>
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public FileResult GetFileZip(cFileID param)
        {
            try
            {
                cFileZip objFile = new cFileZip();
                var result = this._service.GetFileZip(param);
                objFile = result;
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                return File(objFile.objFile, objFile.sFileType, objFile.sFileName);
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
                using (MemoryStream fs = new MemoryStream())
                {
                    fs.Position = 0;
                    // Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                    return File(fs.ToArray(), "application/zip", "AllFileZip.zip");
                }
            }
        }
        /// <summary>
        /// GetAllInprogress
        /// </summary>
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult GetAllInprogress(objGetData obj)
        {
            var result = this._service.GetAllInprogress(obj);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// SaveDataRoom
        /// </summary>

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult SaveDataRoom(cSaveRoom param)
        {
            var result = this._service.SaveDataRoom(param);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// GetAllInprogress
        /// </summary>
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult GetDataTableFileALL(cFilterTable param)
        {
            var result = this._service.GetDataTableFileALL(param);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// DeleteRoom
        /// </summary>
        [HttpGet]
        // [ValidateAntiForgeryToken]
        public IActionResult DeleteRoom(int nID)
        {
            var result = this._service.DeleteRoom(nID);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// GetData
        /// </summary>
        [HttpGet]
        public IActionResult GetDataRoom(int nID)
        {
            var result = this._service.GetDataRoom(nID);
            return StatusCode(result.nStatusCode, result);
        }

         /// <summary>
        /// Cancel MT
        /// </summary>
        [HttpPost]
        public IActionResult Cancel(cReqDataOT param)
        {
            var result = _service.Cancel(param);
            return StatusCode(result.nStatusCode, result);
        }
    }
}

