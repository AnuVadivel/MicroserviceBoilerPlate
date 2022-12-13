using Xunit;

namespace Payment.UnitTests.Persistence
{
    // this is a way to just all DB stuff in one class InMemoryDbContextFixture 
    // and pass in all related test classes and decorate our test class with [Collection("InMemDB")] attribute
    // we can create different such fixture classes as per our use case
    [CollectionDefinition("InMemDB")]
    public class InMemoryDbContextFixtureCollection : ICollectionFixture<InMemoryDbContextFixture>
    {
    }
}