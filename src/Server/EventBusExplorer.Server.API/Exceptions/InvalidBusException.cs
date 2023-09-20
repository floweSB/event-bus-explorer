namespace EventBusExplorer.Server.API.Exceptions;

/// <summary>
/// Exception to represent a not consistent input with
/// the current configuration
/// </summary>
public class InvalidBusException : Exception
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="invalidName">The bus name given as input</param>
    public InvalidBusException(string invalidName)
        : base($"Invalid bus name {invalidName}")
    { }
}

/// <summary>
/// Exception to represent a missing event bus in
/// configuration
/// </summary>
public class MissingBusException : Exception
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public MissingBusException()
        : base("More than a event bus found in configuration, but headers don't have anyone")
    { }
}
