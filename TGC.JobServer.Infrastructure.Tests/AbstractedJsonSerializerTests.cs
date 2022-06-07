using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace TGC.JobServer.Infrastructure.Tests
{
    public class AbstractedJsonSerializerTests
    {
        private static string TestJsonString = "{\"value1\":\"value1\",\"value2\":2}";

        [Fact]
        public void Deserialize_ReturnsParsedGenericType()
        {
            //Arrange
            var serializer = new AbstractedJsonSerializer();

            //Act
            var resultObject = serializer.Deserialize<TestSerializerObject>(TestJsonString);

            //Assert
            resultObject.Should().BeOfType<TestSerializerObject>();
            resultObject.value1.Should().Be("value1");
            resultObject.value2.Should().Be(2);
        }

        [Fact]
        public void Deserialize_ReturnsSameGenericObject()
        {
            var serializer = new AbstractedJsonSerializer();
            var testJsonDocument = JsonDocument.Parse(TestJsonString);

            //Act
            var resultObject1 = serializer.Deserialize<TestSerializerObject>(TestJsonString);
            var resultObject2 = serializer.Deserialize<TestSerializerObject>(testJsonDocument);

            //Assert
            resultObject1.Should().BeOfType<TestSerializerObject>();
            resultObject2.Should().BeOfType<TestSerializerObject>();

            resultObject1.value1.Should().Be("value1");
            resultObject2.value1.Should().Be("value1");
            resultObject1.value2.Should().Be(2);
            resultObject2.value2.Should().Be(2);

            resultObject1.Should().BeEquivalentTo(resultObject2);
        }
    }

    internal class TestSerializerObject
    {
        public string value1 { get; set; }
        public int value2 { get; set; }
    }
}
