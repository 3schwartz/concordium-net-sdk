using System.Text.Json;
using Concordium.Sdk.Types.Mapped;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Concordium.Sdk.Examples;

public sealed class GetBlockTransactionEvents : Tests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetBlockTransactionEvents(ITestOutputHelper output) : base(output) =>
        this._jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new BlockItemSummaryDetailsConverter(), new UpdatePayloadConverter() }
        };

    [Fact]
    public async Task RunGetBlockTransactionEvents()
    {
        var idx = 0UL;
        var transactionCount = 0;
        while (transactionCount < 2)
        {
            var blockHeight = new Absolute(idx);
            await foreach (var transaction in this.Client.GetBlockTransactionEvents(blockHeight))
            {
                transactionCount++;
                var serialized = JsonSerializer.Serialize(transaction, this._jsonSerializerOptions);
                this.Output.WriteLine(serialized);
            }
            idx++;
        }
    }
}

internal sealed class BlockItemSummaryDetailsConverter : System.Text.Json.Serialization.JsonConverter<IBlockItemSummaryDetails>
{
    public override IBlockItemSummaryDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IBlockItemSummaryDetails value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}

internal sealed class UpdatePayloadConverter : System.Text.Json.Serialization.JsonConverter<IUpdatePayload>
{
    public override IUpdatePayload? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IUpdatePayload value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}