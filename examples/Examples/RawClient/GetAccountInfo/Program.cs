﻿using Concordium.Sdk.Client;
using Concordium.Grpc.V2;
using Concordium.Sdk.Examples.Common;

namespace Concordium.Sdk.Examples.RawClient;

/// <summary>
/// Example demonstrating the use of <see cref="Client.RawClient.GetAccountInfo"/>.
///
/// <see cref="RawClient"/> wraps methods of the Concordium Node GRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library. Creating an instance
/// of the generated <see cref="AccountInfoRequest"/> class used for the method input is given below.
/// </summary>
class Program
{
    static void GetAccountInfo(GetAccountInfoExampleOptions options)
    {
        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60 // Use a timeout of 60 seconds.
        );

        BlockHashInput blockHashInput;

        switch (options.BlockHash.ToLower())
        {
            case "best":
                blockHashInput = new BlockHashInput() { Best = new Empty() };
                break;
            case "lastfinal":
                blockHashInput = new BlockHashInput() { LastFinal = new Empty() };
                break;
            default:
                blockHashInput = Concordium.Sdk.Types.BlockHash
                    .From(options.BlockHash)
                    .ToBlockHashInput();
                break;
        }

        // Construct the input for the "raw" method.
        AccountInfoRequest request = new AccountInfoRequest
        {
            /// Convert command line parameter to a <see cref="Concordium.Sdk.Types.BlockHash"/>
            /// and then to a <see cref="BlockHashInput"/> which is needed for the <see cref="AccountInfoRequest"/>.
            BlockHash = blockHashInput,
            /// Convert command line parameter to a <see cref="Concordium.Sdk.Types.AccountAddress"/>
            /// and then to a <see cref="AccountIdentifierInput"/> which is needed for the <see cref="AccountInfoRequest"/>.
            AccountIdentifier = Concordium.Sdk.Types.AccountAddress
                .From(options.AccountAddress)
                .ToAccountIdentifierInput()
        };

        // Invoke the "raw" call.
        AccountInfo accountInfo = client.Raw.GetAccountInfo(request);

        // Print account info.
        PrintAccountInfo(accountInfo);
    }

    static void PrintAccountInfo(AccountInfo accountInfo)
    {
        Console.WriteLine(
            $@"
            Address:          {Concordium.Sdk.Types.AccountAddress .From(accountInfo.Address.Value.ToArray()) .ToString()}
            Balance:          {accountInfo.Amount.Value.ToString()} CCD
            Sequence number:  {accountInfo.SequenceNumber.Value.ToString()}
        "
        );
    }

    static void Main(string[] args)
    {
        Example.Run<GetAccountInfoExampleOptions>(args, GetAccountInfo);
    }
}