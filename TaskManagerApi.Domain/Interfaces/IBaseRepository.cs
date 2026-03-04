using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Entities;

namespace TaskManagerApi.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetValuesAsync();
        Task<T?> FindValueAsync(int id);
        Task<T> AddAsync(T data);
        Task UpdateAsync(T data);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllByIdAsync(Expression<Func<T, bool>> condition);
        Task<T?> GetAsync(Expression<Func<T, bool>> condition);
    }
}
