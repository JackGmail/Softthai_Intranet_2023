using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Backend.Models.ExampleModel;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ExampleController : ControllerBase
    {
        private readonly IExampleService _service;
        public ExampleController(IExampleService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Save ข้อมูลของ Form Example
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveData(cExampleData param)
        {
            var result = await this._service.SaveData(param);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// Get ข้อมูลของ Form
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var result = await this._service.GetData();
            return StatusCode(result.nStatusCode, result);
        }
    }

}
