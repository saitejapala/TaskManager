using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Infrastructure.Data;

namespace TaskManagerApi.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        public BaseRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T AddData)
        {
            await _dbContext.Set<T>().AddAsync(AddData);
            await _dbContext.SaveChangesAsync();
            return AddData;
        }

        public async Task DeleteAsync(int deleteId)
        {
            var deleteData = await _dbContext.Set<T>().FindAsync(deleteId);
            if (deleteData != null)
            {
                _dbContext.Set<T>().Remove(deleteData);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<T?> FindValueAsync(int FindId)
        {
            return await _dbContext.Set<T>().FindAsync(FindId);
        }

        public async Task<IEnumerable<T>> GetValuesAsync()
        {
            return await _dbContext.Set<T>().ToListAsync() ?? [];
        }
        public async Task<IEnumerable<T>> GetAllByIdAsync(Expression<Func<T,bool>> condition)
        {
            return await _dbContext.Set<T>().Where(condition).ToListAsync();
        }
        public async Task<T?> GetAsync(Expression<Func<T,bool>> condition)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(condition);
        }

        public async Task UpdateAsync(T UpdateData)
        {
            _dbContext.Set<T>().Update(UpdateData);
            await _dbContext.SaveChangesAsync();
        }
    }
}
