using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Entities;   
namespace TaskManagerApi.Domain.Interfaces
{
    public interface IWorkItemRepository
    {
        Task<IEnumerable<WorkItem>> GetAllAsync();
        Task<WorkItem?> GetByIdAsync(int id);
        Task AddAsync(WorkItem workItem);
        Task UpdateAsync(WorkItem workItem);
        Task DeleteAsync(int id);
    }
}
