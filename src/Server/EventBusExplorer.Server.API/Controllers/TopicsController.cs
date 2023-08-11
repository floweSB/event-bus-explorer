using System.Net.Mime;
using EventBusExplorer.Server.Application;
using Microsoft.AspNetCore.Mvc;

namespace EventBusExplorer.Server.API.Controllers;

/// <summary>
/// API group to manage topics
/// </summary>
[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class TopicsController : ControllerBase
{
    private readonly IEventBusManagementService _eventBusManagementService;
    private readonly IMessagesService _messagesService;

    public TopicsController(
        IEventBusManagementService eventBusManagementService,
        IMessagesService messagesService)
    {
        _eventBusManagementService = eventBusManagementService ??
            throw new ArgumentNullException(nameof(eventBusManagementService));

        _messagesService = messagesService ??
            throw new ArgumentNullException(nameof(messagesService));
    }

    /// <summary>
    /// Get list of topics
    /// </summary>
    /// <response code="200">Returns the list of topic names</response>
    [ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetTopicsAsync()
    {
        IList<string> queueNames = await _eventBusManagementService.GetTopicsAsync();
        GetTopicsResponse response = new(queueNames);

        return Ok(response);
    }

    /// <summary>
    /// Get details of the given topic
    /// </summary>
    /// <param name="name">Topic name</param>
    /// <response code="200">Return details of the given queue</response>
    [ProducesResponseType(typeof(GetTopicResponse), StatusCodes.Status200OK)]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetTopicAsync([FromRoute] string name)
    {
        string queueName = await _eventBusManagementService.GetTopicAsync(name);
        GetTopicResponse response = new(queueName);
        return Ok(response);
    }

    /// <summary>
    /// Create a new topic
    /// </summary>
    /// <param name="createRequest">Properties of the new topic</param>
    /// <response code="200">Location of the created topic</response>
    [ProducesResponseType(typeof(CreateTopicResponse), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> CreateTopicAsync(
        [FromBody] CreateTopicRequest createRequest)
    {
        string queueName = await _eventBusManagementService.CreateTopicAsync(createRequest.Name);
        CreateTopicResponse queueResponse = new(queueName);
        return Ok(queueResponse);
    }

    /// <summary>
    /// Delete the specified topic
    /// </summary>
    /// <param name="name">Name of the topic to delete</param>
    /// <response code="204">No content</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteTopicAsync(
        [FromRoute] string name)
    {
        await _eventBusManagementService.DeleteTopicAsync(name);
        return NoContent();
    }

    /// <summary>
    /// Get subscriptions of the given topic
    /// </summary>
    /// <param name="topicName">Topic to retrieve subscriptions</param>
    /// <response code="200">List of subscriptions</response>
    [ProducesResponseType(typeof(GetTopicSubscriptionsResponse), StatusCodes.Status200OK)]
    [HttpGet("{topicName}/subscriptions")]
    public async Task<IActionResult> GetSubscriptionsAsync(string topicName)
    {
        IList<string> subscriptionNames = await _eventBusManagementService.GetTopicSubscriptionsAsync(topicName);
        GetTopicSubscriptionsResponse response = new(subscriptionNames);
        return Ok(response);
    }

    /// <summary>
    /// Get details of the given subscription
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Subscription name</param>
    /// <returns></returns>
    [HttpGet("{topicName}/subscriptions/{subscriptionName}")]
    public async Task<IActionResult> GetSubscriptionAsync([FromRoute] string topicName, [FromRoute] string subscriptionName)
    {
        string name = await _eventBusManagementService.GetTopicSubscriptionAsync(topicName, subscriptionName);
        GetTopicSubscriptionResponse response = new(name);
        return Ok(response);
    }

    /// <summary>
    /// Create a new subscription in the specified topic
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="createRequest">Properties of the subscription to create</param>
    /// <response code="200">Location of the created topic</response>
    [ProducesResponseType(typeof(CreateTopicResponse), StatusCodes.Status200OK)]
    [HttpPost("{topicName}/subscriptions")]
    public async Task<IActionResult> CreateSubscriptionAsync(
        [FromRoute] string topicName,
        [FromBody] CreateSubscriptionRequest createRequest)
    {
        string subscriptionName = await _eventBusManagementService.CreateTopicSubscriptionAsync(topicName, createRequest.Name);
        CreateSubscriptionResponse response = new(subscriptionName);
        return Ok(response);
    }

    /// <summary>
    /// Delete the specified subscription in the given topic
    /// </summary>
    /// <param name="topicName">Name of the topic</param>
    /// <param name="subscriptionName">Name of the subscription to delete</param>
    /// <response code="204">No content</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{topicName}/subscriptions/{subscriptionName}")]
    public async Task<IActionResult> DeleteSubscriptionAsync(
        [FromRoute] string topicName,
        [FromRoute] string subscriptionName)
    {
        await _eventBusManagementService.DeleteTopicSubscriptionAsync(topicName, subscriptionName);
        return NoContent();
    }

    /// <summary>
    /// Peek messages in topic subscription
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Topic subscription name</param>
    /// <param name="receiveMode">Receive mode</param>
    /// <param name="subQueue">Sub queue to query</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <response code="200">List of peeked messages</response>
    [ProducesResponseType(typeof(GetMessagesResponse), StatusCodes.Status200OK)]
    [HttpGet("{topicName}/subscriptions/{subscriptionName}/messages")]
    public async Task<IActionResult> GetMessagesAsync(
        [FromRoute] string topicName,
        [FromRoute] string subscriptionName,
        [FromQuery] ReceiveMode receiveMode,
        [FromQuery] SubQueue subQueue,
        [FromQuery] long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        GetMessagesResponse dto = await _messagesService.GetMessagesAsync(
            topicName,
            subscriptionName,
            new QuerySettings(receiveMode, subQueue),
            fromSequenceNumber,
            cancellationToken);

        return Ok(dto);
    }

    /// <summary>
    /// Purge messages in topic subscription
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Topic subscription name</param>
    /// <param name="subQueue">Sub queue to query</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <response code="204">No content</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{topicName}/subscriptions/{subscriptionName}/messages")]
    public async Task<IActionResult> GetMessagesAsync(
        [FromRoute] string topicName,
        [FromRoute] string subscriptionName,
        [FromQuery] SubQueue subQueue,
        CancellationToken cancellationToken = default)
    {
        await _messagesService.PurgeMessagesAsync(topicName, subscriptionName, subQueue, cancellationToken);

        return NoContent();
    }
}
