using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerApi.Models
{
    public record ResponseModel(bool IsSuccess, object? Data = null, string Message = "Something went wrong");

}
