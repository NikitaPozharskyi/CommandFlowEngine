using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;
using CommandFlowEngine.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CommandFlowEngine.DependencyProvider;

public class WorkflowAndCommandDependencyProvider : IWorkflowAndCommandDependencyProvider
{
    private readonly IServiceProvider _serviceProvider;

    public WorkflowAndCommandDependencyProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWorkflow<TRequest, TResponse> GetWorkflow<TRequest, TResponse>(Type workflowType) where TRequest : IRequest
    {
        return _serviceProvider.GetRequiredService(workflowType) as IWorkflow<TRequest, TResponse> ??
               throw new InvalidWorkflowException("Requested type is not a Workflow");
    }

    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType) where TRequest : IRequest
    {
        return _serviceProvider.GetRequiredService(commandType) as ICommand<TRequest, TResponse> ??
               throw new InvalidCommandException("Requested type is not a Command");
    }
}