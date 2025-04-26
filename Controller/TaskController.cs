using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zimozi.Assessment.BusinessModels.RequestModels.Response;
using Zimozi.Assessment.BusinessModels.ResponseModels;
using Zimozi.Assessment.Models;
using Zimozi.Assessment.Services.TaskService;

namespace Zimozi.Assessment.Controller
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _taskService.GetById(id);
            if(result.Data.UserId != int.Parse(userId) && role != Role.Admin.ToString())
            {
                return StatusCode(401, new BusinessResult<string>("You don't have any permission to access this resource", 401, null));
            }
            return StatusCode(result.StatusCode, result);

        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (id != int.Parse(userId) && role != Role.Admin.ToString())
            {
                return StatusCode(401, new BusinessResult<string>("You don't have any permission to access this resource", 401, null));
            }
            var result = await _taskService.GetByUserId(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost()]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> Create([FromBody] CreateTaskModel model)
        {
            var result = await _taskService.CreateTask(model);
            return StatusCode(result.StatusCode, result);
        }
    }
}
