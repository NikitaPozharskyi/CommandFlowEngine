using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;

namespace CommandFlowEngine.DependencyProvider;

public interface IWorkflowAndCommandDependencyProvider
{
    public IWorkflow<TRequest, TResponse> GetWorkflow<TRequest, TResponse>(Type workflowType)
        where TRequest: IRequest;
    
    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType)
        where TRequest: IRequest;

}