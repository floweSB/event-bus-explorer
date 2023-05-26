using System.Net.Mime;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
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
    private readonly IServiceBrokerQueuesService _queueService;

    public QueuesController(IServiceBrokerQueuesService queueService)
    {
        _queueService = queueService;
    }

    /// <summary>
    /// Get lsit of queues
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET: /queues
    /// 
    /// </remarks>
    /// <response code="200">Returns the list of queue names</response>
    [ProducesResponseType(typeof(GetQueuesResponse), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<string> queueNames = await _queueService.GetAsync();
        GetQueuesResponse response = new(queueNames);

        return Ok(response);
    }

    /// <summary>
    /// Get details of the given queue
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET: /queues/{name}
    /// 
    /// </remarks>
    /// <param name="name">Queue name</param>
    /// <response code="200">Return details of the given queue</response>
    [ProducesResponseType(typeof(GetQueueResponse), StatusCodes.Status200OK)]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync([FromRoute] string name)
    {
        string queueName = await _queueService.GetAsync(name);
        GetQueueResponse response = new(queueName);
        return Ok(response);
    }

    /// <summary>
    /// Create a new queue
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST: /queues
    /// 
    /// </remarks>
    /// <param name="createRequest">Properties of the new queue</param>
    /// <response code="200">Location of the created queue</response>
    [ProducesResponseType(typeof(CreateQueueResponse), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateQueueRequest createRequest)
    {
        string queueName = await _queueService.CreateAsync(createRequest.Name);
        CreateQueueResponse queueResponse = new(queueName);
        return Ok(queueResponse);
    }

    /// <summary>
    /// Delete the specified queue
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE: /queues
    /// 
    /// </remarks>
    /// <param name="name">Name of the queue to delete</param>
    /// <response code="204">No content</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] string name)
    {
        await _queueService.DeleteAsync(name);
        return NoContent();
    }
}
