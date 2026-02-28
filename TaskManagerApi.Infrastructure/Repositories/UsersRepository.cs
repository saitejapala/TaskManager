using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Infrastructure.Data;

namespace TaskManagerApi.Infrastructure.Repositories
{
    public class UsersRepository : BaseRepository<Users>, IUsersRepository
    {
        private readonly AppDbContext _context;
        public UsersRepository(AppDbContext context) :base(context) 
        {
            _context = context;
        }
    }
}
