using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using Graylog.Contracts.Responses;
using Graylog.Contracts.Exceptions;

namespace Graylog.API.Filters
{
    public class CustomExceptionFilter : IAsyncExceptionFilter, IFilterMetadata
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            #region Variables
            string errorMessage = string.Empty;
            HttpStatusCode statusCode;
            Exception ex = context.Exception;
            string exceptionName = ex.GetType().Name;
            #endregion


            if (ex is ArgumentNullException argumentNullException)
            {
                errorMessage = argumentNullException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (ex is NotFoundException notFoundException)
            {
                errorMessage = notFoundException.Message;
                statusCode = HttpStatusCode.NotFound;
            }
            else if (ex is IOException ioException)
            {
                errorMessage = ioException.Message;
                statusCode = HttpStatusCode.NotFound;
            }
            else if (ex is ValidationException validationException)
            {
                errorMessage = validationException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (ex is BadRequestException badRequestException)
            {
                errorMessage = badRequestException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                errorMessage = "Default CustomExceptionFilter Error Message";
                statusCode = HttpStatusCode.InternalServerError;
            }

            var errorResponse = new ErrorDataResponse(new List<string> { errorMessage }, statusCode);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

    }
}
