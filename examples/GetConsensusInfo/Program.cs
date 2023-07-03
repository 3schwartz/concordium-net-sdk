﻿using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace Example;

internal sealed class GetConsensusInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com/:20000")]
    public Uri Uri { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetConsensusInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args)
    {
        await Parser.Default
            .ParseArguments<GetConsensusInfoOptions>(args)
            .WithParsedAsync(options => Run(options));
    }

    static async Task Run(GetConsensusInfoOptions options) {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new BlockHashConverter() }
        };

        var poolStatus = await client.GetConsensusInfoAsync();

        var serialized = JsonSerializer.Serialize(poolStatus, jsonSerializerOptions);
        Console.WriteLine(serialized);
    }
}

internal sealed class BlockHashConverter : JsonConverter<BlockHash>
{
    public override BlockHash? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, BlockHash value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}
