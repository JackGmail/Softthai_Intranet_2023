using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.AsyncAutoDataTest
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestToolsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthentication _authen;
        private readonly ITestToolsService _service;
        public TestToolsController(IConfiguration config, IAuthentication authen, ITestToolsService service)
        {
            _config = config;
            _authen = authen;
            _service = service;
        }

        #region Async Auto Complete
        [HttpGet]
        public IActionResult SearchAllData(string strSearch)
        {
            var result = this._service.SearchAllData(strSearch);
            return Ok(result);
        }
        #endregion
    }
}
