using System.Text;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus.Helpers;

internal static class MessagesHelper
{
    public static string ReadMessage(BinaryData binaryData) =>
        Encoding.UTF8.GetString(binaryData);
}
