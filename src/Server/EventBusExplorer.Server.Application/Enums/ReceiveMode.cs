namespace EventBusExplorer.Server.Application;

/// <summary>
/// Instruct messages fetch mode
/// </summary>
/// <remarks>
/// Default option is Peek
/// </remarks>
public enum ReceiveMode
{
    /// <summary>
    /// Messages are copied and retrivied
    /// </summary>
    Peek = 0,

    /// <summary>
    /// Messages are retrieved and removed from entity
    /// </summary>
    Receive = 1,
}
