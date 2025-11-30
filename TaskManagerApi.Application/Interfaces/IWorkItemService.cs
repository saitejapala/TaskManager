using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Application.Dtos;

namespace TaskManagerApi.Application.Interfaces
{
    public interface IWorkItemService
    {
        Task<IEnumerable<WorkItemDto>> GetAllTasksAsync();
        Task<WorkItemDto?> GetTaskByIdAsync(int id);
        Task<WorkItemDto> CreateTaskAsync(CreateWorkItemDto createWorkItemDto);
        Task<bool> UpdateTaskAsync(int id, CreateWorkItemDto updateWorkItemDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}
