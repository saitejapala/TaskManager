using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Entities;   
namespace TaskManagerApi.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<Users?> GetUserByEmailAsync(string email);
        Task<Users?> GetByIdAsync(int id);
        Task<Users> AddAsync(Users users);
        Task UpdateAsync(Users users);
        Task DeleteAsync(int id);
    }
}
