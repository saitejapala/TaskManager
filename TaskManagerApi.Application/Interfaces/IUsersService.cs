using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Domain.Entities;

namespace TaskManagerApi.Application.Interfaces
{
    public interface IUsersService
    {
        Task<SignUpDto?> GetUserByEmailAsync(string email);
        Task<SignUpDto?> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(SignUpDto signUpDto);
        Task<bool> UpdateTaskAsync(int id, SignUpDto signUpDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}
