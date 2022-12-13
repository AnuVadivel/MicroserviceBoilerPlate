using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.Framework.Api.Error;
using Payment.Framework.Api.Filter;
using Payment.Framework.Client.Web.Exceptions;
using Payment.Framework.Shared.Exception;
using Xunit;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace Payment.Framework.UnitTests.Api.Filter
{
    public class PaymentExceptionFilterAttributeTests
    {
        private readonly PaymentExceptionFilterAttribute _filter;

        public PaymentExceptionFilterAttributeTests()
        {
            var logger = new Mock<ILogger<PaymentExceptionFilterAttribute>>().Object;
            _filter = new PaymentExceptionFilterAttribute(logger);
        }
        
        [Theory]
        [MemberData(nameof(Exceptions))]
        public void GivenAnyHandledExceptionRaisedShouldSetErrorDetailsInContext(
            Exception ex, string expectedCode, HttpStatusCode expectedStatus)
        {
            var exceptionContext = CreateExceptionContext(ex);
            
            _filter.OnException(exceptionContext);

            var errorResponse = ErrorResponseReader(exceptionContext);
            errorResponse.Description.Should().BeEquivalentTo(ex.Message);
            errorResponse.Code.Should().BeEquivalentTo(expectedCode);
            errorResponse.Status.Should().Be(expectedStatus);
        }


        private static ExceptionContext CreateExceptionContext(Exception exception) =>
            new(
                    new ActionContext
                    {
                        HttpContext = new DefaultHttpContext(),
                        RouteData = new RouteData(),
                        ActionDescriptor = new ActionDescriptor()
                    }, new List<IFilterMetadata>())
                { Exception = exception };

        private static FlurlHttpException CreateFlurlException(HttpStatusCode httpStatusCode)
        {
            var httpCall = new FlurlCall
            {
                Response = new FlurlResponse(new HttpResponseMessage(httpStatusCode))
            };

            return new FlurlHttpException(httpCall, "test-exception", new Exception("test-exception"));
        }

        public static IEnumerable<object[]> Exceptions =>
            new List<object[]>
            {
                new object[]
                {
                    new NotFoundException("Not exist"),
                    nameof(HttpStatusCode.NotFound),
                    HttpStatusCode.NotFound
                },
                new object[]
                {
                    new Payment.Framework.Shared.Exception.ArgumentException("Bad Request"),
                    nameof(HttpStatusCode.BadRequest),
                    HttpStatusCode.BadRequest
                },
                new object[]
                {
                    new Exception("Something went wrong"),
                    nameof(HttpStatusCode.InternalServerError),
                    HttpStatusCode.InternalServerError
                },
                new object[]
                {
                    new HttpException(HttpStatusCode.BadRequest, "some http error"),
                    nameof(HttpStatusCode.BadRequest),
                    HttpStatusCode.BadRequest
                },
                new object[]
                {
                    new WebClientException("Something went wrong"),
                    nameof(HttpStatusCode.InternalServerError),
                    HttpStatusCode.InternalServerError
                },
                new object[]
                {
                    new WebClientException("forbiddenAccess", CreateFlurlException(HttpStatusCode.Forbidden)),
                    nameof(HttpStatusCode.Forbidden),
                    HttpStatusCode.Forbidden
                },
                new object[]
                {
                    new WebClientException("unauthorizedAccess", CreateFlurlException(HttpStatusCode.Unauthorized)),
                    nameof(HttpStatusCode.Unauthorized),
                    HttpStatusCode.Unauthorized
                }
            };


        private static ErrorDescription ErrorResponseReader(ExceptionContext exceptionContext) =>
            ((exceptionContext.Result as ObjectResult)?.Value as ErrorResponse)?.Error;
    }
}