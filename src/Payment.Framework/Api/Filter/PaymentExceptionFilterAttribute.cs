using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Payment.Framework.Api.Error;
using Payment.Framework.Client.Web.Exceptions;
using Payment.Framework.Shared.Exception;
using Payment.Framework.Shared.Extension;
using static System.String;
using ArgumentException = Payment.Framework.Shared.Exception.ArgumentException;

namespace Payment.Framework.Api.Filter
{
    /// <summary>
    /// Default exception filter for teh api. It will catch all exception, log it and return error response 
    /// </summary>
    public class PaymentExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public PaymentExceptionFilterAttribute(ILogger<PaymentExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        protected virtual void HandleException(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception;

            if (HandleWebClientException(exceptionContext, exception))
                return;

            var (httpCode, errorCode, message) = exception switch
            {
                ArgumentException _ => (HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), Empty),
                BadRequestException _ => (HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), Empty),
                NotFoundException _ => (HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), Empty),
                ConflictException _ => (HttpStatusCode.Conflict, HttpStatusCode.Conflict.ToString(), Empty),
                ForbiddenException _ => (HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString(), Empty),
                UnAuthorisedException _ => (HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString(), Empty),
                HttpException ex => (ex.HttpStatusCode, ex.HttpStatusCode.ToString(), Empty),
                _ => (HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.ToString(),
                    "Something went wrong")
            };

            WriteError(exceptionContext, CreateErrorResponse(httpCode, errorCode, message, exception));
        }

        private bool HandleWebClientException(ExceptionContext exceptionContext, Exception exception)
        {
            if (exception is WebClientException clientException)
                try
                {
                    var errorResponse = clientException.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => new ErrorResponse
                        {
                            Error = new ErrorDescription
                            {
                                Status = clientException.StatusCode.Value,
                                Code = HttpStatusCode.Unauthorized.ToString(),
                                Description = clientException.Message
                            }
                        },
                        HttpStatusCode.Forbidden => new ErrorResponse
                        {
                            Error = new ErrorDescription
                            {
                                Status = clientException.StatusCode.Value,
                                Code = HttpStatusCode.Forbidden.ToString(),
                                Description = clientException.Message
                            }
                        },
                        HttpStatusCode.NotFound => new ErrorResponse
                        {
                            Error = new ErrorDescription
                            {
                                Status = clientException.StatusCode.Value,
                                Code = HttpStatusCode.NotFound.ToString(),
                                Description = clientException.Message
                            }
                        },
                        HttpStatusCode.UnprocessableEntity => new ErrorResponse
                        {
                            Error = new ErrorDescription
                            {
                                Status = clientException.StatusCode.Value,
                                Code = HttpStatusCode.UnprocessableEntity.ToString(),
                                Description = clientException.Message
                            }
                        },
                        _ => clientException.GetResponseJsonAsync<ErrorResponse>().Result
                    };

                    if (errorResponse != null)
                    {
                        WriteError(exceptionContext, errorResponse);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Parsing ErrorResponse from WebClientException failed");
                }

            return false;
        }

        private static ErrorResponse CreateErrorResponse(HttpStatusCode httpCode, string errorCode, string message,
            Exception exception)
        {
            var error = new ErrorResponse
            {
                Error = new ErrorDescription
                {
                    Status = httpCode,
                    Code = errorCode,
                    Description = message.IsNullOrWhiteSpace() ? exception.Message : message
                }
            };
            return error;
        }

        protected virtual void WriteError(ExceptionContext exceptionContext, ErrorResponse error)
        {
            exceptionContext.Result = new ObjectResult(error)
            {
                StatusCode = (int?)error.Error.Status
            };

            _logger.LogError(exceptionContext.Exception, $"Sending ErrorResponse: {error}");
        }
    }
}