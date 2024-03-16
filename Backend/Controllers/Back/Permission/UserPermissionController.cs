using Backend.Interfaces.Back.Permission;
using Backend.Models;
using Backend.Models.Back.Permission;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Back.Permission
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUserPermission _service;
        public UserPermissionController(IUserPermission service)
        {
            this._service = service;
        }
        [HttpGet]
        public IActionResult GetOptionInitialData()
        {
            var result = this._service.GetOptionInitialData();
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public IActionResult GetEmployee(string? strSearch)
        {
            var result = this._service.GetEmployee(strSearch);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult GetDataTable(cReqUserTable param)
        {
            var result = this._service.GetDataTable(param);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        public IActionResult GetUserPerForm(UserPerModelRequest param)
        {
            var result = this._service.GetUserPerForm(param);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult SaveDataUser(cResultDataUserPer param)
        {
            var result = this._service.SaveDataUser(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult DeleteDataUser(cReqDeleteUser param)
        {
            var result = this._service.DeleteDataUser(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult UserToggleActive(ClassTableUser param)
        {
            var result = this._service.UserToggleActive(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult GetPermissionUserGroup(UserModelRequest param)
        {
            var result = this._service.GetPermissionUserGroup(param);
            return StatusCode(result.nStatusCode, result);
        }

    }
}
