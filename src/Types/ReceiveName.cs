namespace Concordium.Sdk.Types;

/// <summary>
/// A receive name of the contract function called. Expected format: "&lt;contract_name&gt;.&lt;func_name&gt;".
/// </summary>
public readonly record struct ReceiveName(string Receive);
