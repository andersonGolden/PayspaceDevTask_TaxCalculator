using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Core.Interfaces;

namespace TaxCalculator.Core.Utilities
{
    public class Result : IResult
    {
        public Result()
        {
        }

        public string message_Id { get; set; }

        public string message { get; set; }

        public bool succeeded { get; set; }
    }
    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }
        public T data { get; set; }

        public DateTime servertime { get; set; } = DateTime.UtcNow;

        public new static Result<T> Fail(string message, string message_id)
        {
            return new Result<T> { succeeded = false, message = message, message_Id = message_id };
        }

        public new static Task<Result<T>> FailAsync(string message, string message_id)
        {
            return Task.FromResult(Fail(message, message_id));
        }


        public static Result<T> Success(T data, string message, string message_id)
        {
            return new Result<T> { succeeded = true, data = data, message = message, message_Id = message_id };
        }

        public static Task<Result<T>> SuccessAsync(T data, string message, string message_id)
        {
            return Task.FromResult(Success(data, message, message_id));
        }
    }
}
