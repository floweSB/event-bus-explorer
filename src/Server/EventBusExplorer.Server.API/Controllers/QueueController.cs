using System.Net.Mime;
using AppInfra = EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EventBusExplorer.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class QueuesController : ControllerBase
{
    private readonly AppInfra.IServiceBrokerQueueService _queueService;

    public QueuesController(AppInfra.IServiceBrokerQueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        AppInfra.GetQueuesResponse queues = await _queueService.GetAsync();
        GetQueuesResponse response = new(queues.Items.Select(x => new GetQueuesResponseItem(x.Name)).ToList());

        return Ok(response);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync([FromRoute] string name)
    {
        AppInfra.GetQueueResponse queue = await _queueService.GetAsync(name);
        GetQueueResponse response = new(queue.Name);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateQueueRequest createRequest)
    {
        AppInfra.CreateQueueResponse q = await _queueService.CreateAsync(createRequest.Name);
        CreateQueueResponse q1 = new(q.Name);
        return Ok(q1);
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] string name)
    {
        await _queueService.DeleteAsync(name);
        return NoContent();
    }
}
