using Backend.Interfaces.Permission.UserRole;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using ST_API.Models.ITypeleaveService;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _service;

        /// <summary>
        /// </summary>
        public UserRoleController(IUserRoleService service)
        {
            this._service = service;
        }

        /// <summary>
        /// </summary>

        #region ออร์ Role
        [HttpPost]
        public IActionResult GetDataTable(cUserRoleTable param)
        {
            var result = this._service.GetDataTable(param);
            return StatusCode(result.Status, result);
        }
        [HttpPost]
        public IActionResult GetUserRole(UserRoleModelRequest param)
        {
            var result = this._service.GetUserRole(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult SaveUserRole(UserRoleModel param)
        {
            var result = this._service.SaveUserRole(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult RemoveDataTable(cUserGroupTable param)
        {
            var result = this._service.RemoveDataTable(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult UserRoleToggleActive(ClassTableUserRole param)
        {
            var result =  this._service.UserRoleToggleActive(param);
            return StatusCode(result.nStatusCode, result);
        }
        #endregion
    }
}
