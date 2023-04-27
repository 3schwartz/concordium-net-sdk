using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.SignKey;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class TransactionTestHelpers<T>
    where T : AccountTransactionPayload<T>
{
    public static PreparedAccountTransaction<T> CreatePreparedAccountTransaction(T transaction)
    {
        AccountAddress sender = AccountAddress.From(
            "3QuZ47NkUk5icdDSvnfX8HiJzCnSRjzi6KwGEmqgQ7hCXNBTWN"
        );
        AccountNonce nonce = new AccountNonce(123);
        Expiry expiry = Expiry.From(65537);
        return transaction.Prepare(sender, nonce, expiry);
    }

    public static SignedAccountTransaction<T> CreateSignedTransaction(
        PreparedAccountTransaction<T> preparedTransaction
    )
    {
        // Create a signer.
        Ed25519SignKey key00 = Ed25519SignKey.From(
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda"
        );
        Ed25519SignKey key01 = Ed25519SignKey.From(
            "68d7d0f3ae0581fd9b2b1c47daf1c9c7b5b8eddf3e48e4984ee16ca3c7efea32"
        );
        Ed25519SignKey key11 = Ed25519SignKey.From(
            "ebaf15cfd4182c98fdb81882591c9e96cf459870ebd1a0dda84288a7f9ab9211"
        );
        TransactionSigner signer = new TransactionSigner();
        signer.AddSignerEntry(0, 0, key00);
        signer.AddSignerEntry(0, 1, key01);
        signer.AddSignerEntry(1, 1, key11);

        // Sign the transfer using the signer.
        return preparedTransaction.Sign(signer);
    }
}