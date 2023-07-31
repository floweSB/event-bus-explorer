namespace EventBusExplorer.Server.Application;

/// <summary>
/// Instruct which sub queue must be fetched
/// </summary>
/// <remarks>
/// Default option is Active
/// </remarks>
public enum SubQueue
{
    /// <summary>
    /// Active messages are returned
    /// </summary>
    Active = 0,

    /// <summary>
    /// Dead lettering messages are returned
    /// </summary>
    DeadLetter = 1,
}
