﻿using System.IO;
using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.AnonymityRevokerInfoResponse;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.CustomJsonSerializerTests;

public class AnonymityRevokerInfoTests
{
    // todo: think how to implement it for all response types and for diff json data that is stored in file
    [Fact]
    public async Task Should_correctly_deserialize_json_data()
    {
        // Arrange
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, @"CustomJsonSerializerTests/Files/anonymity-revoker-info.json");
        var expectedJson = await File.ReadAllTextAsync(filePath);

        // Act
        var obj = CustomJsonSerializer.Deserialize<AnonymityRevokerInfo>(expectedJson);
        var actualJson = CustomJsonSerializer.Serialize(obj);
        var expected = JToken.Parse(expectedJson);
        var actual = JToken.Parse(actualJson);

        // Assert
        expected.Should().BeEquivalentTo(actual);
    }
}