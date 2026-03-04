using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Infrastructure.Data;

namespace TaskManagerApi.Infrastructure.Repositories
{
    public class WorkItemRepository : BaseRepository<WorkItem>,IWorkItemRepository
    {
        private readonly AppDbContext _context;
        public WorkItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
