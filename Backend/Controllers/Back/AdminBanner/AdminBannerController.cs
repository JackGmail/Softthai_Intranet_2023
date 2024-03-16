using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Extensions.Common.STFunction;
using Backend.Models;

namespace Backend.Controllers
{
    /// <summary>
    /// </summary>

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminBannerController : ControllerBase
    {
        private readonly IAdminBannerSevice _service;

        /// <summary>
        /// </summary>
        public AdminBannerController(IAdminBannerSevice service)
        {
            this._service = service;
        }

        /// <summary>
        /// </summary>
        [HttpGet]
        public IActionResult GetDataTable([FromQuery] ClsFilterBanner obj)
        {

            var result = _service.GetDataTable(obj);
            return StatusCode(result.Status, result);
        }
    

        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult OnSave(ObjSave bannerSave)
        {

            var result = _service.OnSave(bannerSave);
            return StatusCode(result.nStatusCode, result);
        }


        /// <summary>
        /// </summary>

        [HttpGet]
        public IActionResult GetDataBanner(string sID)
        {

            var result = _service.GetDataBanner(sID);
            return StatusCode(result.nStatusCode, result);
        }

        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult OnDelete(ClsFilterDelete bannerFilter)
        {

            var result = _service.OnDelete(bannerFilter);
            return StatusCode(result.nStatusCode, result);
        }

        /// <summary>
        /// </summary>
        [HttpPost]
        public IActionResult OnChangeOrder(STFunction.ChgFilter bannerFilter)
        {

            var result = _service.OnChangeOrder(bannerFilter);
            return StatusCode(result.nStatusCode, result);
        }

         [HttpPost]
        public IActionResult BannerActive(ClassTableGroup param)
        {
            var result = this._service.BannerActive(param);
            return StatusCode(result.nStatusCode, result);
        }

    }
}