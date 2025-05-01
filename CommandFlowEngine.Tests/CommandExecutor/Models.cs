using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;

namespace CommandFlowEngine.Tests.CommandExecutor;

public class Request : IRequest
{
    public Request(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}

public class Response : IResponse
{
    public string Message { get; set; }
}

public class FirstCommand : ICommand<Request, Response>
{
    public Queue<IWorkflow<Request, Response>> Workflows { get; set; }

    public FirstCommand()
    {
        Workflows = new Queue<IWorkflow<Request, Response>>();
        Workflows.Enqueue(new FirstWorkflow());
        Workflows.Enqueue(new SecondWorkflow());
    }

    public FirstCommand(Queue<IWorkflow<Request, Response>> workflows)
    {
        Workflows = workflows;
    }

    public Task<Response> ExecuteAsync(Request request)
    {
        return Task.FromResult(new Response { Message = $"{request.Message} command executed" });
    }
}

public class SecondCommand : ICommand<Request, Response>
{
    public Queue<IWorkflow<Request, Response>> Workflows { get; set; }

    public SecondCommand()
    {
        Workflows = new Queue<IWorkflow<Request, Response>>();
        Workflows.Enqueue(new SecondWorkflow());
        Workflows.Enqueue(new FirstWorkflow());
    }

    public SecondCommand(Queue<IWorkflow<Request, Response>> workflows)
    {
        Workflows = workflows;
    }
    public Task<Response> ExecuteAsync(Request request)
    {
        return Task.FromResult(new Response { Message = $"{request.Message} command executed" });
    }
}

public class FirstWorkflow : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request message)
    {
        return Task.FromResult(new Response { Message = $"first workflow executed: {message.Message}" });
    }
}

public class SecondWorkflow : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request message)
    {
        return Task.FromResult(new Response { Message = $"second workflow executed: {message.Message}" });
    }
}