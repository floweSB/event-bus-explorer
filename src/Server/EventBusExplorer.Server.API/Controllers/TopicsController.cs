﻿using System.Net.Mime;
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
    public async Task<IActionResult> GetTopicsAsync()
    {
        IList<string> queueNames = await _topicService.GetTopicsAsync();
        GetTopicsResponse response = new(queueNames);

        return Ok(response);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetTopicAsync([FromRoute] string name)
    {
        string queueName = await _topicService.GetTopicAsync(name);
        GetTopicResponse response = new(queueName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTopicAsync(
        [FromBody] CreateTopicRequest createRequest)
    {
        string queueName = await _topicService.CreateTopicAsync(createRequest.Name);
        CreateTopicResponse queueResponse = new(queueName);
        return Ok(queueResponse);
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteTopicAsync(
        [FromRoute] string name)
    {
        await _topicService.DeleteTopicAsync(name);
        return NoContent();
    }

    [HttpGet("{topicName}/subscriptions")]
    public async Task<IActionResult> GetSubscriptionsAsync(string topicName)
    {
        IList<string> subscriptionNames = await _topicService.GetSubscriptionsAsync(topicName);
        GetTopicSubscriptionsResponse response = new(subscriptionNames);
        return Ok(response);
    }

    [HttpGet("{topicName}/subscriptions/{subscriptionName}")]
    public async Task<IActionResult> GetSubscriptionAsync([FromRoute] string topicName, [FromRoute] string subscriptionName)
    {
        string name = await _topicService.GetSubscriptionAsync(topicName, subscriptionName);
        GetTopicSubscriptionResponse response = new(name);
        return Ok(response);
    }

    [HttpPost("{topicName}/subscriptions")]
    public async Task<IActionResult> CreateSubscriptionAsync(
        [FromRoute] string topicName,
        [FromBody] CreateSubscriptionRequest createRequest)
    {
        string subscriptionName = await _topicService.CreateSubscriptionAsync(topicName, createRequest.Name);
        CreateSubscriptionResponse response = new(subscriptionName);
        return Ok(response);
    }

    [HttpDelete("{topicName}/subscriptions/{subscriptionName}")]
    public async Task<IActionResult> DeleteSubscriptionAsync(
        [FromRoute] string topicName,
        [FromRoute] string subscriptionName)
    {
        await _topicService.DeleteSubscriptionAsync(topicName, subscriptionName);
        return NoContent();
    }
}
