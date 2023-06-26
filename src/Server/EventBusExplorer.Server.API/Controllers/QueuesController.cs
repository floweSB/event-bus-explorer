using System.Net.Mime;
using EventBusExplorer.Server.Application;
using Microsoft.AspNetCore.Mvc;

namespace EventBusExplorer.Server.API.Controllers;

/// <summary>
/// API group to manage queues
/// </summary>
[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class QueuesController : ControllerBase
{
    private readonly IEventBusManagementService _eventBusManagementService;
    private readonly IMessagesService _messagesService;

    public QueuesController(
        IEventBusManagementService eventBusManagementService,
        IMessagesService messagesService)
    {
        _eventBusManagementService = eventBusManagementService ??
            throw new ArgumentNullException(nameof(eventBusManagementService));

        _messagesService = messagesService ??
            throw new ArgumentNullException(nameof(messagesService));
    }

    /// <summary>
    /// Get list of queues
    /// </summary>
    /// <response code="200">Returns the list of queue names</response>
    [ProducesResponseType(typeof(GetQueuesResponse), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<string> queueNames = await _eventBusManagementService.GetQueuesAsync();
        GetQueuesResponse response = new(queueNames);

        return Ok(response);
    }

    /// <summary>
    /// Get details of the given queue
    /// </summary>
    /// <param name="name">Queue name</param>
    /// <response code="200">Return details of the given queue</response>
    [ProducesResponseType(typeof(GetQueueResponse), StatusCodes.Status200OK)]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync([FromRoute] string name)
    {
        string queueName = await _eventBusManagementService.GetQueueAsync(name);
        GetQueueResponse response = new(queueName);
        return Ok(response);
    }

    /// <summary>
    /// Create a new queue
    /// </summary>
    /// <param name="createRequest">Properties of the new queue</param>
    /// <response code="200">Location of the created queue</response>
    [ProducesResponseType(typeof(CreateQueueResponse), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateQueueRequest createRequest)
    {
        string queueName = await _eventBusManagementService.CreateQueueAsync(createRequest.Name);
        CreateQueueResponse queueResponse = new(queueName);
        return Ok(queueResponse);
    }

    /// <summary>
    /// Delete the specified queue
    /// </summary>
    /// <param name="name">Name of the queue to delete</param>
    /// <response code="204">No content</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] string name)
    {
        await _eventBusManagementService.DeleteQueueAsync(name);
        return NoContent();
    }

    /// <summary>
    /// Peek messages in queue
    /// </summary>
    /// <param name="queueName">Queue name</param>
    /// <param name="receiveMode">Receive mode</param>
    /// <param name="subQueue">Sub queue to query</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <response code="200">List of peeked messages</response>
    [ProducesResponseType(typeof(GetMessagesResponse), StatusCodes.Status200OK)]
    [HttpGet("{queueName}/messages")]
    public async Task<IActionResult> PeekMessagesAsync(
        [FromRoute] string queueName,
        [FromQuery] ReceiveMode receiveMode,
        [FromQuery] SubQueue subQueue,
        [FromQuery] long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        GetMessagesResponse dto = await _messagesService.GetMessagesAsync(
            queueName,
            new QuerySettings(receiveMode, subQueue),
            fromSequenceNumber,
            cancellationToken);

        return Ok(dto);
    }
}
