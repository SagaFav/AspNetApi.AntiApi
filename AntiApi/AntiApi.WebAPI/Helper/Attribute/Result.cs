using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anti.Api.Model
{

    public class Result
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static Result ErrorResult(string message)
        {
            return new Result()
            {
                Success = false,
                Message = message
            };
        }

        public static Result ErrorResult(string code, string message)
        {
            return new Result()
            {
                Success = false,
                Message = message,
                Code = code,
            };
        }

        public static Result SuccessResult(string message)
        {
            return new Result()
            {
                Success = true,
                Message = message
            };
        }
    }

}