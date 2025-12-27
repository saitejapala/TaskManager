using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Domain.Entities
{
    public class WorkItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false; 
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }

        public Users? User { get; set; }
    }
}
