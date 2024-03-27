﻿using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.TestApplication.Models;

namespace CommandWorkflows.TestApplication;

public class TestWorkflow2 : IWorkflow<MyRequest, MyResponse>
{
    public Task<MyResponse> ExecuteAsync(MyRequest message)
    {
        Console.WriteLine("TestWorkflow 2");

        Console.WriteLine($"Executing workflow... {message.Message}");
    
        return Task.FromResult(new MyResponse
        {
            Message = message.Message
        });
    }
}