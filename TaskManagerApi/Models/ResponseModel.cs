using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerApi.Models
{
    public class ResponseModel
    {
        public ResponseModel(HttpStatusCode StatusCode, bool IsSuccess, string Data = "", string Message = "Something went wrong" )
        {
            this.StatusCode = (int)StatusCode;
            this.IsSuccess = IsSuccess;
            this.Data = Data;
            this.Message = IsSuccess ? string.Empty : Message;
        }
        public int StatusCode { get; internal set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public bool IsSuccess { get; internal set; }
    }
}
