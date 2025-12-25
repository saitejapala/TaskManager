using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApi.Application.Dtos
{
    public class AuthDto
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }   
    }
}
