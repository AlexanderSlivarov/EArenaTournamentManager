using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Common.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public List<Error>? Errors { get; set; }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Errors = null
            };
        }

        public static ServiceResult<T> Failure(T? data, List<Error> errors)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Data = data,
                Errors = errors
            };
        }
    }
}
