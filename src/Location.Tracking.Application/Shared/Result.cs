using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success()
        {
            return new Result(true,null);
        }

        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }
    }
    public class Result<T> : Result
    {
        public T? Data { get; }

        protected Result(bool isSuccess, Error? error, T? data) : base(isSuccess, error)
        {
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
