using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Payment.Framework.Api.Filter;
using Payment.Framework.Shared.Exception;
using Xunit;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace Payment.Framework.UnitTests.Api.Filter
{
    public class ModelValidationFilterTest
    {
        private readonly Mock<ActionExecutionDelegate> _next;
        private readonly ModelValidationFilter _modelValidationFilter;
        private readonly ActionContext _actionContext;
        private readonly ActionExecutingContext _actionExecutingContext;

        public ModelValidationFilterTest()
        {
            _next = new Mock<ActionExecutionDelegate>();
            _modelValidationFilter = new ModelValidationFilter();
            var modelState = new ModelStateDictionary();
            _actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            _actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );
        }

        [Fact]
        public async Task ShouldThrowExceptionIfModelIsInValid()
        {
            _actionContext.ModelState.AddModelError("test", "Bad request");

            await _modelValidationFilter.Invoking(y => y.OnActionExecutionAsync(_actionExecutingContext, _next.Object))
                .Should().ThrowAsync<BadRequestException>().WithMessage("Bad request").WithMessage("Bad request");
        }
        [Fact]
        public void ShouldNotThrowExceptionIfModelIsValid()
        {
            _modelValidationFilter.Invoking(y => y.OnActionExecutionAsync(_actionExecutingContext, _next.Object))
                .Should().NotThrowAsync();
        }
    }
}