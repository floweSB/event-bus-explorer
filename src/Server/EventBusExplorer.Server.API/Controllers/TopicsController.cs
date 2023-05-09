using System.Net.Mime;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EventBusExplorer.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class TopicsController : ControllerBase
{
    private readonly IServiceBrokerTopicsService _topicService;

    public TopicsController(IServiceBrokerTopicsService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<string> queueNames = await _topicService.GetAsync();
        GetQueuesResponse response = new(queueNames);

        return Ok(response);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync([FromRoute] string name)
    {
        string queueName = await _topicService.GetAsync(name);
        GetQueueResponse response = new(queueName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateQueueRequest createRequest)
    {
        string queueName = await _topicService.CreateAsync(createRequest.Name);
        CreateQueueResponse queueResponse = new(queueName);
        return Ok(queueResponse);
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] string name)
    {
        await _topicService.DeleteAsync(name);
        return NoContent();
    }
}
