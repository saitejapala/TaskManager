using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkItemsController : Controller
    {
        private readonly IWorkItemService _workItemService;
        public WorkItemsController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<ResponseModel>> GetAllTasks()
        {
            var tasks = await _workItemService.GetAllTasksAsync();
            return Ok(new ResponseModel(IsSuccess: true, Data: tasks));
        }

        [HttpGet("Details")]
        public async Task<ActionResult<ResponseModel>> Details([FromQuery]int id)
        {
            var task = await _workItemService.GetTaskByIdAsync(id);
            if (task is null)
            {
                return NotFound(new ResponseModel(IsSuccess: false, Message: "No data found"));
            }
            return Ok(new ResponseModel(true, task));
        }

        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Create([FromBody] CreateWorkItemDto dto)
        {
            var createdTask = await _workItemService.CreateTaskAsync(dto);
            if (createdTask is null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseModel (IsSuccess: false, Message: "Task not craeted"));
            }
            return StatusCode((int)HttpStatusCode.Created,new ResponseModel(true, createdTask));
        }

        [HttpPut("UpdateTask")]
        public async Task<ActionResult<ResponseModel>> UpdateTask([FromQuery]int id, CreateWorkItemDto dto)
        {
            var updatedTask = await _workItemService.UpdateTaskAsync(id, dto);
            if (!updatedTask)
            {
                return NotFound(new ResponseModel(IsSuccess: false, Message: "Task not found"));
            }
            return Ok(new ResponseModel(IsSuccess: true, Data: id));
        }

        [HttpDelete("DeleteTask")]
        public async Task<ActionResult<ResponseModel>> DeleteTask([FromQuery] int id)
        {
            var deletedTask = await _workItemService.DeleteTaskAsync(id);
            if (!deletedTask)
            {
                return NotFound(new ResponseModel(IsSuccess: false, Message: "Task not deleted"));
            }
            return Ok(new ResponseModel(IsSuccess: true, Data: id));
        }
    }
}
