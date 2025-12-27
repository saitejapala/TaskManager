using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Infrastructure.Data;

namespace TaskManagerApi.Infrastructure.Repositories
{
    public class WorkItemRepository : IWorkItemRepository
    {
        private readonly AppDbContext _context;
        public WorkItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<WorkItem>> GetAllAsync(int id)
        {
            return await _context.WorkItems.Where(e => e.UserId == id).ToListAsync();
        }
        public async Task<WorkItem?> GetByIdAsync(int id)
        {
            return await _context.WorkItems.FindAsync(id);
        }
        public async Task AddAsync(WorkItem workItem)
        {
            await _context.WorkItems.AddAsync(workItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(WorkItem workItem)
        {
            _context.WorkItems.Update(workItem);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var item = await _context.WorkItems.FindAsync(id);
            if (item != null)
            {
                _context.WorkItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
