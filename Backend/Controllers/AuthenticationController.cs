using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authen;

        /// <summary>
        /// </summary>
        public AuthenticationController(IAuthentication authen)
        {
            _authen = authen;
        }

        /// <summary>
        /// เข้าสู่ระบบ
        /// </summary>
        /// <param name="param"></param>
        /// <response code="200">ผ่าน</response>
        /// <response code="401">ไม่ผ่าน</response>
        /// <response code="500">Internal Server Error Check Log</response>
        [HttpPost]
        public IActionResult Login(ParamLogin param)
        {
            ResultAPI result = _authen.Login(param);
            return StatusCode(result.nStatusCode, result);
        }

        /// <summary>
        /// Get MenuPermission
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult GetMenuPermission(string sRoute)
        {
            ResultAPI result = _authen.GetMenuPermission(sRoute);
            return StatusCode(result.nStatusCode, result);
        }

        
        [HttpPost]
        public IActionResult LoginAccountLine(AccountLine Param)
        {
            var result = _authen.LoginAccountLine(Param);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AutoLoginFromLine(AccountLine Param)
        {
            var result = _authen.AutoLoginFromLine(Param);
            return Ok(result);
        }
        
        [HttpPost]
        public IActionResult CheckActionAlreadyLine(AccountLine Param)
        {
            var result = _authen.CheckActionAlreadyLine(Param);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult STEncrypt(STEnCrypt Param)
        {
            var result = _authen.STEncrypt(Param);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult STDecrypt(STEnCrypt Param)
        {
            var result = _authen.STDecrypt(Param);
            return Ok(result);
        }

    }
}
