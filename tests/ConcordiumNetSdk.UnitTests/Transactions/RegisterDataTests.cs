using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class RegisterDataTests
{
    /// <summary>
    /// Creates a new instance of the <see cref="RegisterData"/>
    /// transaction with the hex encoded string "feedbeef" as its
    /// value.
    /// </summary>
    public static RegisterData CreateRegisterData()
    {
        var data = Data.From("feedbeef");
        return new RegisterData(data);
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var expectedBytes = new byte[] { 21, 0, 4, 254, 237, 190, 239 };
        CreateRegisterData().GetBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void Prepare_ThenSign_ProducesCorrectSignatures()
    {
        // Create the transfer.
        RegisterData transfer = CreateRegisterData();

        // Prepare the transfer.
        PreparedAccountTransaction<RegisterData> preparedTransfer =
            TransactionTestHelpers<RegisterData>.CreatePreparedAccountTransaction(transfer);

        // Sign the transfer.
        SignedAccountTransaction<RegisterData> signedTransfer =
            TransactionTestHelpers<RegisterData>.CreateSignedTransaction(preparedTransfer);

        // Create the expected signature.
        byte[] expectedSignature00 = Convert.FromHexString(
            "4e611658eb4d70c35cf35a959b4cf6f4da8dc94da0f3cf900d39ced627253e5ac137af6a01ebae9d4c0131829c656fa5fcebab01282df4b464daae73c467a303"
        );
        byte[] expectedSignature01 = Convert.FromHexString(
            "7cbdc17785ff3dca2e5d165970e7276603cc3d00ea53a2dc507b14552fea68d645dc399fe70264f65d3f242ff8a6ed4ea862e7b24f55036592456b079cf27b07"
        );
        byte[] expectedSignature11 = Convert.FromHexString(
            "c7274b080e606d19656c2c18308463bffda70d16df927c05ae2ed2d8679f39dbe1d77bb0bd21cf29b06d62f7485a740e4d2d46baa9e7a494a96da115144d0604"
        );

        var expectedSignature = TransactionTestHelpers<RegisterData>.FromExpectedSignatures(
            expectedSignature00,
            expectedSignature01,
            expectedSignature11
        );

        signedTransfer.Signature.signatureMap
            .Should()
            .BeEquivalentTo(expectedSignature.signatureMap);
    }
}