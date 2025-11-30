using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerApi.Controllers
{
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
        public async Task<ResponseModel> GetAllTasks()
        {
            var tasks = await _workItemService.GetAllTasksAsync();
            return new ResponseModel(HttpStatusCode.OK, true, JsonSerializer.Serialize(tasks));
        }

        [HttpGet("Details/{id}")]
        public async Task<ResponseModel> Details(int id)
        {
            var task = await _workItemService.GetTaskByIdAsync(id);
            if (task is null)
            {
                return new ResponseModel(HttpStatusCode.NotFound, false, Message:"No data found");
            }
            return new ResponseModel(HttpStatusCode.OK, true, JsonSerializer.Serialize(task));
        }
        
        [HttpPost]
        public async Task<ResponseModel> Create([FromBody] CreateWorkItemDto dto)
        {
            var createdTask = await _workItemService.CreateTaskAsync(dto);
            if (createdTask is null)
            {
                return new ResponseModel(HttpStatusCode.InternalServerError, false, Message: "Task not craeted");
            }
            return new ResponseModel(HttpStatusCode.Created, true, JsonSerializer.Serialize(createdTask));
        }

        [HttpPut("UpdateTask/{id}")]
        public async Task<ResponseModel> UpdateTask( int id, CreateWorkItemDto dto)
        {
            var updatedTask = await _workItemService.UpdateTaskAsync(id, dto);
            if (updatedTask)
            {
                return new ResponseModel(HttpStatusCode.InternalServerError, false, Message: "Task not updated");
            }
            return new ResponseModel(HttpStatusCode.OK, true, JsonSerializer.Serialize(updatedTask));
        }
        
        [HttpDelete("DeleteTask/{id}")]
        public async Task<ResponseModel> DeleteTask(int id)
        {
            var deletedTask = await _workItemService.DeleteTaskAsync(id);
            if (deletedTask)
            {
                return new ResponseModel(HttpStatusCode.InternalServerError, false, Message: "Task not deleted");
            }
            return new ResponseModel(HttpStatusCode.OK, true, JsonSerializer.Serialize(deletedTask));
        }
    }
}
