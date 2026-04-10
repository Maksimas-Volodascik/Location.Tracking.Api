using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }
        public T? Data { get; }

        protected Result(bool isSuccess, Error? error, T? data)
        {
            IsSuccess = isSuccess;
            Error = error;
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, null, data);
        }

        public static Result<T> Failure(Error error)
        {
            return new Result<T>(false, error, default);
        }
    }
}
