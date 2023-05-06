namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an account key index.
///
/// An account has one or more credentials, each identified by a unique
/// credential index (modeled by <see cref="AccountCredentialIndex"/>).
///
/// For each credential the account may have up to 255 keys, where each
/// such key is identified by another unique byte-value referred to as
/// its account key index. This index is relative to the credential index
/// of the credential to which it belongs. Thus a pair of a credential
/// index of the account and the key index of a key belonging to the
/// corresponding credential uniquely identifies that key.
/// </summary>
public readonly struct AccountKeyIndex
{
    public readonly byte Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountKeyIndex"/> class.
    /// </summary>
    /// <param name="value">An account key index represented by a <see cref="byte"/>.</param>
    public AccountKeyIndex(byte value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an instance from a string representing a <see cref="byte"/> value.
    /// </summary>
    /// <param name="index">An index represented as a string representing to be parsed as a <see cref="byte"/> value.</param>
    public static AccountKeyIndex From(string index)
    {
        byte result;
        if (Byte.TryParse(index, out result))
        {
            return new AccountKeyIndex(result);
        }
        throw new ArgumentException("Could not parse the account key index.");
    }

    public static implicit operator AccountKeyIndex(byte value)
    {
        return new AccountKeyIndex(value);
    }

    public static implicit operator byte(AccountKeyIndex accountKeyIndex)
    {
        return accountKeyIndex.Value;
    }
}
