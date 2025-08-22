using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Errors
{
    public class RestException : Exception
    {
        public int Code { get; set; }   
        public string? Message { get; set; }
        public List<RestExceptionError> Errors { get; set; } = new List<RestExceptionError>();  
        public RestException(int code, string key,string errorMessage, string message = null)
        {
            this.Code = code;
            this.Message = message;
            this.Errors = new List<RestExceptionError>() { new RestExceptionError { Key = key,ErrorMessage = errorMessage} };
        }
        public RestException(int code,string errorMessage)
        {
            Code = code;
            this.Errors = new List<RestExceptionError>() { new RestExceptionError {ErrorMessage = errorMessage} };
        }
    }
    public class RestExceptionError
    {
        public string? Key { get; set; }
        public string? ErrorMessage { get; set; }
        public RestExceptionError(string key,string errorMessage)
        {
            this.Key = key;
            this.ErrorMessage = errorMessage;
        }
        public RestExceptionError()
        {
            
        }
    }

}
