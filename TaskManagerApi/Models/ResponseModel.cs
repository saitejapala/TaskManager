using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerApi.Models
{
    public class ResponseModel
    {
        public ResponseModel(bool IsSuccess, object? Data = null, string Message = "Something went wrong" )
        {
            this.IsSuccess = IsSuccess;
            this.Data = Data;
            this.Message = IsSuccess ? string.Empty : Message;
        }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
    }
}
