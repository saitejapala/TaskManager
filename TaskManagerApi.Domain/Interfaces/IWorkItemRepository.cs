using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Entities;   
namespace TaskManagerApi.Domain.Interfaces
{
    public interface IWorkItemRepository : IBaseRepository<WorkItem>
    {

    }
}
