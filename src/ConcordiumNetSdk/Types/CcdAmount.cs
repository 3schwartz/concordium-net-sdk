﻿using ConcordiumNetSdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a CCD amount.
///
/// Note that 1_000_000 µCCD is equal to 1 CCD.
/// </summary>
public readonly struct CcdAmount : IEquatable<CcdAmount>
{
    public const int BytesLength = 8;

    /// <summary>
    /// Conversion factor, 1_000_000 µCCD = 1 CCD.
    /// </summary>
    public const UInt64 MicroCcdPerCcd = 1_000_000;

    /// <summary>
    /// The amount in µCCD.
    /// </summary>
    public readonly UInt64 Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="CcdAmount"/> class.
    /// </summary>
    /// <param name="microCcd">The amount in µCCD.</param>
    private CcdAmount(UInt64 microCcd)
    {
        Value = microCcd;
    }

    /// <summary>
    /// Get a formatted string representing the amount in µCCD.
    /// </summary>
    public string GetFormattedMicroCcd()
    {
        return $"{Value}";
    }

    /// <summary>
    /// Get a formatted string representing the amount in CCD.
    /// </summary>
    public string GetFormattedCcd()
    {
        return $"{Value / (decimal)MicroCcdPerCcd}";
    }

    /// <summary>
    /// Creates an instance from a µCCD amount represented as an integer.
    /// </summary>
    /// <param name="microCcd">µCCD amount represented as an integer.</param>
    public static CcdAmount FromMicroCcd(UInt64 microCcd)
    {
        return new CcdAmount(microCcd);
    }

    /// <summary>
    /// Creates an instance from a CCD amount represented by an integer.
    /// </summary>
    /// <param name="ccd">CCD amount represented by an integer.</param>
    /// <exception cref="ArgumentException">The CCD amount in µCCD does not fit in <see cref="UInt64"/></exception>
    public static CcdAmount FromCcd(UInt64 ccd)
    {
        try
        {
            return new CcdAmount(checked(ccd * MicroCcdPerCcd));
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {ccd} CCD * {MicroCcdPerCcd} µCCD/CCD does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Add CCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">The result odoes not fit in <see cref="UInt64"/></exception>
    public static CcdAmount operator +(CcdAmount a, CcdAmount b)
    {
        try
        {
            UInt64 newAmount = checked(a.Value + b.Value);
            return CcdAmount.FromMicroCcd(newAmount);
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {a.Value} + {b.Value} does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Subtract CCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">The result does not fit in <see cref="UInt64"/></exception>
    public static CcdAmount operator -(CcdAmount a, CcdAmount b)
    {
        try
        {
            UInt64 newAmount = checked(a.Value - b.Value);
            return CcdAmount.FromMicroCcd(newAmount);
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {a.Value} - {b.Value} does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Get the CCD amount in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes()
    {
        return Serialization.GetBytes(this.Value);
    }

    public bool Equals(CcdAmount other)
    {
        return Value == other.Value;
    }

    public override bool Equals(Object? obj)
    {
        return obj is CcdAmount other && Equals(other);
    }

    public static bool operator ==(CcdAmount? left, CcdAmount? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CcdAmount? left, CcdAmount? right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
