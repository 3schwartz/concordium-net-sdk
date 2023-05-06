using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an amount of energy.
/// </summary>
public readonly struct EnergyAmount
{
    public const UInt32 BytesLength = sizeof(UInt64);
    public readonly UInt64 Value { get; init; }

    public EnergyAmount(UInt64 value)
    {
        Value = value;
    }

    public static implicit operator EnergyAmount(UInt64 value)
    {
        return new EnergyAmount(value);
    }

    public static implicit operator UInt64(EnergyAmount value)
    {
        return value.Value;
    }

    /// <summary>
    /// Get the energy amount in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes()
    {
        return Serialization.GetBytes(Value);
    }
}
