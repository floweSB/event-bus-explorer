using System.Diagnostics;

namespace EventBusExplorer.Server.Application;

/// <summary>
/// Set of settings used to query messages
/// </summary>
/// <param name="ReceiveMode">Desired receive mode</param>
/// <param name="SubQueue">Desired sub queue</param>
[DebuggerDisplay("{ReceiveMode} - {SubQueue}")]
public record QuerySettings(ReceiveMode ReceiveMode, SubQueue SubQueue);
