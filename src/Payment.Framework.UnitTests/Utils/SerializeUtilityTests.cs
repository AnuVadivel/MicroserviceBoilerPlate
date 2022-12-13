using FluentAssertions;
using Payment.Framework.Shared.Util;
using Xunit;

namespace Payment.Framework.UnitTests.Utils
{
    public class SerializeUtilityTests
    {
        private  class TestData
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Role { get; set; }
        }

        
        private const string JsonString =
            "{\"name\":\"test\",\"id\":\"1234\",\"role\":null}";
        
        [Fact]
        public void Serialize_GivenAnObject_ReturnsSerializedString()
        {
            var input = new TestData
            {
                Id = "1234",
                Name = "test"
            };

            SerializeUtility.Serialize(input)
                .Should()
                .BeEquivalentTo(JsonString);
        }

        [Fact]
        public void DeSerialize_GivenAString_ReturnsDeserializeObject()
        {
            SerializeUtility.Deserialize<TestData>(JsonString)
                .Should()
                .BeEquivalentTo(new TestData
                {
                    Id = "1234",
                    Name = "test"
                });
        }
        
        [Fact]
        public void NS_Serialize_GivenAnObject_ReturnsSerializedString()
        {
            var input = new TestData
            {
                Id = "1234",
                Name = "test"
            };

            NewtonSoftSerializeUtility.Serialize(input)
                .Should()
                .BeEquivalentTo(JsonString);
        }

        [Fact]
        public void NS_DeSerialize_GivenAString_ReturnsDeserializeObject()
        {
            NewtonSoftSerializeUtility.Deserialize<TestData>(JsonString)
                .Should()
                .BeEquivalentTo(new TestData
                {
                    Id = "1234",
                    Name = "test"
                });
        }

        
    }
}