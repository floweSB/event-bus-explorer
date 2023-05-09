using System.Net.Mime;
using EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EventBusExplorer.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class QueuesController : ControllerBase
{
    private readonly IServiceBrokerQueueService _queueService;

    public QueuesController(IServiceBrokerQueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<string> queueNames = await _queueService.GetAsync();
        GetQueuesResponse response = new(queueNames);

        return Ok(response);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync([FromRoute] string name)
    {
        string queueName = await _queueService.GetAsync(name);
        GetQueueResponse response = new(queueName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateQueueRequest createRequest)
    {
        string queueName = await _queueService.CreateAsync(createRequest.Name);
        CreateQueueResponse queueResponse = new(queueName);
        return Ok(queueResponse);
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] string name)
    {
        await _queueService.DeleteAsync(name);
        return NoContent();
    }
}
