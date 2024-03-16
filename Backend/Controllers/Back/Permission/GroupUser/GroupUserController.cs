using Backend.Interfaces.Permission.GroupUser;
using Backend.Interfaces.Permission.UserRole;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        private readonly IGroupUserService _service;

        /// <summary>
        /// </summary>
        public GroupUserController(IGroupUserService service)
        {
            this._service = service;
        }

        #region 
        [HttpGet]
        public IActionResult GetInitData()
        {
            var result = this._service.GetInitData();
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult GetDataTable(cUserGroupTable param)
        {
            var result = this._service.GetDataTable(param);
            return StatusCode(result.Status, result);
        }
        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult GetUserGroup(UserGroupModelRequest param)
        {
            var result = this._service.GetUserGroup(param);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult SaveUserGroup(UserGroupModel param)
        {
            var result = this._service.SaveUserGroup(param);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult RemoveDataGroupTable(cUserGroupTable param)
        {
            var result = this._service.RemoveDataGroupTable(param);
            return StatusCode(result.nStatusCode, result);
        }
        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult GetPermissionUserRole(UserRoleModelRequest param)
        {
            var result = this._service.GetPermissionUserRole(param);
            return StatusCode(result.nStatusCode, result);
        }
        [HttpPost]
        public IActionResult GroupToggleActive(ClassTableGroup param)
        {
            var result = this._service.GroupToggleActive(param);
            return StatusCode(result.nStatusCode, result);
        }
        #endregion
    }
}
