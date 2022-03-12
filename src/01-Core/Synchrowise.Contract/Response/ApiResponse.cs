using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Response
{
    public class ApiResponse <T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccessfull { get; set; }
        public ErrorResponse Error { get; set; }

        public static ApiResponse<T> Success(T Data, int StatusCode){
            return new ApiResponse<T>{
                Data = Data,
                StatusCode = StatusCode,
                IsSuccessfull = true
            };
        }
        public static ApiResponse<T> Success(int StatusCode){
            return new ApiResponse<T>{
                Data = default,
                StatusCode = StatusCode,
                IsSuccessfull = true
            };
        }
        public static ApiResponse<T> Fail(ErrorResponse error, int StatusCode){
            return new ApiResponse<T>{
                Error = error,
                StatusCode= StatusCode,
                IsSuccessfull = false,
            };
        }
        public static ApiResponse<T> Fail(string errorMsg, int statusCode, bool isShow){
            var errorResponse = new ErrorResponse(errorMsg,isShow);
            return new ApiResponse<T>{
                Error = errorResponse,
                StatusCode = statusCode,
                IsSuccessfull = false
            };
        }

    }
}