using Concordium.Sdk.Types;
using System.Security.Cryptography;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents an account transaction which is prepared for signing.
///
/// The transasction is prepared in the sense that it contains information about the
/// <see cref="AccountAddress"/> of the sender, the <see cref="AccountSequenceNumber"/> to use
/// when submitting the transaction as well as its <see cref="Types.Expiry"/>.
/// </summary>
public record PreparedAccountTransaction<T>
    where T : AccountTransactionPayload<T>
{
    /// <summary>
    /// Address of the sender of the transaction.
    /// </summary>
    public AccountAddress Sender { get; init; }

    /// <summary>
    /// Account nonce to use for the transaction.
    /// </summary>
    public AccountSequenceNumber Nonce { get; init; }

    /// <summary>
    /// Expiration time of the transaction.
    /// </summary>
    public Expiry Expiry { get; init; }

    /// <summary>
    /// Payload to send to the node.
    /// </summary>
    public AccountTransactionPayload<T> Payload { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreparedAccountTransaction{T}"/> class.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to be sent to the node.</param>
    public PreparedAccountTransaction(
        AccountAddress sender,
        AccountSequenceNumber nonce,
        Expiry expiry,
        AccountTransactionPayload<T> payload
    )
    {
        this.Sender = sender;
        this.Nonce = nonce;
        this.Expiry = expiry;
        this.Payload = payload;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignedAccountTransaction"/> class.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to send to the node.</param>
    /// <param name="transactionSigner">The signer to use for signing the transaction.</param>

    /// <summary>
    /// Signs the prepared transaction using the provided signer.
    /// </summary>
    /// <param name="signer">The signer to use for signing the transaction.</param>
    public SignedAccountTransaction<T> Sign(ITransactionSigner transactionSigner)
    {
        // Get the serialized payload.
        var serializedPayload = this.Payload.GetBytes();
        var serializedPayloadSize = (uint)serializedPayload.Length;

        // Compute the energy cost.
        var txSpecificCost = this.Payload.GetTransactionSpecificCost();
        var energyCost = CalculateEnergyCost(
            transactionSigner.GetSignatureCount(),
            txSpecificCost,
            AccountTransactionHeader.BytesLength,
            serializedPayloadSize
        );

        // Create the header.
        var header = new AccountTransactionHeader(
            this.Sender,
            this.Nonce,
            this.Expiry,
            energyCost,
            serializedPayloadSize
        );

        // Construct the serialized payload and its digest for signing.
        var serializedHeaderAndTxPayload = header.GetBytes().Concat(serializedPayload).ToArray();
        var signDigest = SHA256.Create().ComputeHash(serializedHeaderAndTxPayload);

        // Sign it.
        var signature = transactionSigner.Sign(signDigest);

        return new(header, this.Payload, signature);
    }

    /// <summary>
    /// Calculates the energy cost associated with processing the transaction.
    /// </summary>
    /// <param name="signatureCount">The number of signatures.</param>
    /// <param name="txSpecificCost">The transaction specific cost.</param>
    /// <param name="headerSize">The size of the header in bytes.</param>
    /// <param name="payloadSize">The size of the payload in bytes.</param>
    private static EnergyAmount CalculateEnergyCost(
        uint signatureCount,
        ulong txSpecificCost,
        uint headerSize,
        uint payloadSize
    )
    {
        const uint costPerSignature = 100;
        const uint costPerHeaderAndPayloadByte = 1;

        var result =
            txSpecificCost
            + (costPerSignature * signatureCount)
            + (costPerHeaderAndPayloadByte * (headerSize + payloadSize));

        return new(result);
    }
}
