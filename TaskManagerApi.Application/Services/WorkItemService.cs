using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Domain.Interfaces;

namespace TaskManagerApi.Application.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository;
        public WorkItemService(IWorkItemRepository workItemRepository)
        {
            _workItemRepository = workItemRepository;
        }
        public async Task<IEnumerable<WorkItemDto>> GetAllTasksAsync(int id)
        {
            var items = await _workItemRepository.GetAllAsync(id);
            return items.Select(i => new WorkItemDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                IsCompleted = i.IsCompleted
            });
        }
        public async Task<WorkItemDto?> GetTaskByIdAsync(int id)
        {
            var item = await _workItemRepository.GetByIdAsync(id);
            if (item is null) return null;
            return new WorkItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsCompleted = item.IsCompleted
            };
        }
        public async Task<WorkItemDto> CreateTaskAsync(CreateWorkItemDto createWorkItemDto)
        {
            var workItem = new Domain.Entities.WorkItem
            {
                Title = createWorkItemDto.Title,
                Description = createWorkItemDto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UserId = createWorkItemDto.UserId
            };
            await _workItemRepository.AddAsync(workItem);
            return new WorkItemDto
            {
                Id = workItem.Id,
                Title = workItem.Title,
                Description = workItem.Description,
                IsCompleted = workItem.IsCompleted
            };
        }
        public async Task<bool> UpdateTaskAsync(int id, CreateWorkItemDto updateWorkItemDto)
        {
            var existingItem = await _workItemRepository.GetByIdAsync(id);
            if (existingItem is null) return false;
            existingItem.Title = updateWorkItemDto.Title;
            existingItem.Description = updateWorkItemDto.Description;
            existingItem.UpdatedAt = DateTime.Now;
            await _workItemRepository.UpdateAsync(existingItem);
            return true;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var existingItem = await _workItemRepository.GetByIdAsync(id);
            if (existingItem is null) return false;
            await _workItemRepository.DeleteAsync(id);
            return true;
        }
    }
}
