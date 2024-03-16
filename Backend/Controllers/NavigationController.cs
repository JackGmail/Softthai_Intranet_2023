using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    /// <summary>
    /// Center Controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly INavigation _service;
        /// <summary>
        /// Center Controller
        /// </summary>
        public NavigationController(INavigation service)
        {
            _service = service;
        }

        /// <summary>
        /// Bredcrumb
        /// </summary>
        [HttpGet]
        public IActionResult GetBreadCrumb(string sRoute, string sLanguages)
        {
            ResultAPI result = _service.GetBreadCrumb(sRoute, sLanguages);
            return StatusCode(result.nStatusCode, result);
        }

        /// <summary>
        /// Menu
        /// </summary>
        [HttpGet]
        public IActionResult GetMenu(int nMenuType)
        {
            var result = _service.GetMenu(nMenuType);
            return StatusCode(result.nStatusCode, result);
        }
    }
}
