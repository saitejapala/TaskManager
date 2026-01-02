using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Domain.Interfaces;

namespace TaskManagerApi.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<SignUpDto?> GetUserByEmailAsync(string email)
        {
            var items = await _usersRepository.GetUserByEmailAsync(email);
            if (items is null) return null;
            return  new SignUpDto
            {
                UserId = items.UserId,
                Email = items.Email,
                Password = items.PasswordHash,
                FullName = items.FullName
            };
        }
        public async Task<SignUpDto?> GetTaskByIdAsync(int id)
        {
            var item = await _usersRepository.GetByIdAsync(id);
            if (item is null) return null;
            return new SignUpDto
            {
                Email = item.Email,
                Password = item.PasswordHash,
                FullName = item.FullName
            };
        }
        public async Task<SignUpDto> CreateTaskAsync(SignUpDto signUpDto)
        {
            var workItem = new Domain.Entities.Users
            {
                Email = signUpDto.Email,
                FullName = signUpDto.FullName,
                PasswordHash = signUpDto.Password,
                CreatedAt = DateTime.Now
            };
            var savedUser = await _usersRepository.AddAsync(workItem);
            return new SignUpDto
            {
                UserId = savedUser.UserId,
                Email = savedUser.Email,
                FullName = savedUser.FullName,
                Password = savedUser.PasswordHash
            };
        }
        public async Task<bool> UpdateTaskAsync(int id, SignUpDto signUpDto)
        {
            var existingItem = await _usersRepository.GetByIdAsync(id);
            if (existingItem is null) return false;
            existingItem.Email = signUpDto.Email;
            existingItem.FullName = signUpDto.FullName;
            existingItem.PasswordHash = signUpDto.Password; // In real scenarios, hash the password
            existingItem.UpdatedAt = DateTime.Now;
            await _usersRepository.UpdateAsync(existingItem);
            return true;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var existingItem = await _usersRepository.GetByIdAsync(id);
            if (existingItem is null) return false;
            await _usersRepository.DeleteAsync(id);
            return true;
        }
    }
}
