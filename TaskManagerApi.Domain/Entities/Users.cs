using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApi.Domain.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }              
        public string Email { get; set; }        
        public string PasswordHash { get; set; } 
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<WorkItem> WorkItems => new List<WorkItem>();
    }
}
