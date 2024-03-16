using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class TasksController : ControllerBase
    {
        private readonly ITasksService _service;
        public TasksController(ITasksService service)
        {
            this._service = service;
        }

        [HttpGet]
        public IActionResult getDentalRemainMoney()
        {
            var result = this._service.getDentalRemainMoney();
            return StatusCode(result.nStatusCode, result);
        }

        [HttpGet]
        public IActionResult LoadDataLeaveSummaryTotalDate()
        {
            var result = this._service.LoadDataLeaveSummaryTotalDate();
            return StatusCode(result.nStatusCode, result);
        }
    }
}