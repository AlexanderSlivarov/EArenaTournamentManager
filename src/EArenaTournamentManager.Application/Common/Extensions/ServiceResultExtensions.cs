using EArenaTournamentManager.Application.Common.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EArenaTournamentManager.Application.Common.Extensions
{
    public class ServiceResultExtensions
    {
        public static ServiceResult<T> Failure<T>(T? data, ModelStateDictionary errors)
        {
            List<Error> errorList = new List<Error>();

            foreach (var item in errors)
            {
                if (item.Value.Errors.Count > 0)
                {
                    errorList.Add(new Error
                    {
                        Key = item.Key,
                        Messages = item.Value.Errors
                                    .Select(e => e.ErrorMessage)
                                    .ToList()
                    });
                }
            }

            return ServiceResult<T>.Failure(data, errorList);
        }
    }
}
