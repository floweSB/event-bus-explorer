using EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EventBusExplorer.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
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
        IList<Queue> queues = await _queueService.GetAsync();
        return Ok(queues);
    }
}
