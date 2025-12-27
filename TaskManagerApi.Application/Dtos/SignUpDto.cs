using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApi.Application.Dtos
{
    public class SignUpDto
    {
        [NotMapped]
        public int UserId { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="InValid email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password id required")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string? FullName { get; set; }
    }
}
