using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Infrastructure.Data;

namespace TaskManagerApi.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Email == email.ToLower());
        }
        public async Task<Users?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<Users> AddAsync(Users users)
        {
            await _context.Users.AddAsync(users);
            await _context.SaveChangesAsync();
            return users;
        }
        public async Task UpdateAsync(Users users)
        {
            _context.Users.Update(users);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
