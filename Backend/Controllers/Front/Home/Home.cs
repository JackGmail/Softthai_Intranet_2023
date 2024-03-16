using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using ST_API.Interfaces;
using ST_API.Models;
using ST_API.Models.IHomeService;
using ST_API.Models.ITypeleaveService;

namespace ST_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _HomeService;
        public HomeController(IHomeService HomeService)
        {
            this._HomeService = HomeService;
        }

        [HttpGet]
        public IActionResult GetDataHomePage()
        {
            var result = this._HomeService.GetDataHomePage();
            return StatusCode(result.Status, result);
        } 
        [HttpGet]
        public IActionResult GetInitData()
        {
            var result = this._HomeService.GetInitData();
            return StatusCode(result.nStatusCode, result);
        }


        /// <summary>
        /// GetBanner
        /// </summary>
        [HttpGet]
        public IActionResult GetBanner()
        {
            var result = this._HomeService.GetBanner();
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public IActionResult PageLoad()
        {
            var result = this._HomeService.PageLoad();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetTableTaskViewManager(ParamViewManager param)
        {
            var result = this._HomeService.GetTableTaskViewManager(param);
            return StatusCode(result.nStatusCode, result);
        }

        [HttpPost]
        public IActionResult GetTableWFHViewManager(ParamViewManager param)
        {
            var result = this._HomeService.GetTableWFHViewManager(param);
            return StatusCode(result.nStatusCode, result);
        } 

        [HttpPost]
        public IActionResult GetTableOTViewManager(ParamViewManager param)
        {
            var result = this._HomeService.GetTableOTViewManager(param);
            return StatusCode(result.nStatusCode, result);
        } 
        [HttpPost]
        public IActionResult GetTableLeaveViewManager(ParamViewManager param)
        {
            var result = this._HomeService.GetTableLeaveViewManager(param);
            return StatusCode(result.nStatusCode, result);
        }

    }

}