using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApi.Application.Dtos
{
    public class CreateWorkItemDto
    {
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public int UserId { get; set; }
    }
}
