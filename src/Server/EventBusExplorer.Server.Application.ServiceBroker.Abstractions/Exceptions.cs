namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

public class ServiceBrokerException : Exception
{
    public ServiceBrokerException()
    {
    }

    public ServiceBrokerException(string message) : base(message)
    {
    }

    public ServiceBrokerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class ServiceBrokerSetupException : ServiceBrokerException
{
    public ServiceBrokerSetupException()
    {
    }

    public ServiceBrokerSetupException(string message) : base(message)
    {
    }

    public ServiceBrokerSetupException(string message, Exception innerException) : base(message, innerException)
    {
    }
}