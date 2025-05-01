using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;
using CommandFlowEngine.DependencyProvider;
using CommandFlowEngine.Extensions;

namespace CommandFlowEngine.TypeResolution;

public class CommandResolver : ICommandResolver
{
    private readonly ICommandTypeResolver _commandTypeResolver;
    private readonly IWorkflowAndCommandDependencyProvider _workflowAndCommandDependencyProvider;

    public CommandResolver(ICommandTypeResolver commandTypeResolver, IWorkflowAndCommandDependencyProvider workflowAndCommandDependencyProvider)
    {
        _commandTypeResolver = commandTypeResolver;
        _workflowAndCommandDependencyProvider = workflowAndCommandDependencyProvider;
    }
    
    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(string commandName, int skip = 0) where TRequest : IRequest
    {
        var command = _workflowAndCommandDependencyProvider.GetCommand<TRequest, TResponse>(_commandTypeResolver.GetCommandType(commandName));
        command.Workflows = InitializeWorkflows<TRequest, TResponse>(command.GetType(), skip);

        return command;
    }
    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType, int skip = 0) where TRequest : IRequest
    {
        var command = _workflowAndCommandDependencyProvider.GetCommand<TRequest, TResponse>(commandType);
        command.Workflows = InitializeWorkflows<TRequest, TResponse>(command.GetType(), skip);

        return command;
    }

    private Queue<IWorkflow<TRequest, TResponse>> InitializeWorkflows<TRequest, TResponse>(Type commandType, int skip) where TRequest : IRequest
    {
        var workflowTypes = _commandTypeResolver.GetRelatedWorkflowsTypeList(commandType);
        
        var workflowQueue = workflowTypes
            .Skip(skip)
            .Select(workflowType => _workflowAndCommandDependencyProvider.GetWorkflow<TRequest, TResponse>(workflowType))
            .ToQueue();

        return workflowQueue;
    }
    
    public bool GetExitCommand<TRequest, TResponse>(string commandName) where TRequest : IRequest
    {
        return _commandTypeResolver.IsCommandExists(commandName) && GetCommand<TRequest, TResponse>(commandName) is IPermanentExitCommand<TRequest, TResponse>;
    }
}