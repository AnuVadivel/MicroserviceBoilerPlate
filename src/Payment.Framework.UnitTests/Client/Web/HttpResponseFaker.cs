using Bogus;

namespace Payment.Framework.UnitTests.Client.Web
{
    public static class HttpResponseFaker
    {
        public static Faker<HttpResponse> Generator { get; } =
            new Faker<HttpResponse>()
                .RuleFor(response => response.ClaimId, f => f.Lorem.Word())
                .RuleFor(response => response.PatientName, f => f.Lorem.Word())
                .RuleFor(response => response.OrderId, f => f.Lorem.Word())
                .RuleFor(response => response.AdjusterId, f => f.Random.Int());
    }
}