namespace CommandFlowEngine.Core.Abstractions;

public abstract class CommandAbstract<TRequest, TResponse> : ICommand<TRequest, TResponse> where TRequest : IRequest
{
    public Queue<IWorkflow<TRequest, TResponse>> Workflows { get; set; } = [];
    
    public abstract Task<TResponse> ExecuteAsync(TRequest request);
}