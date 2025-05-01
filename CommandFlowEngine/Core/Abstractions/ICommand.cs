namespace CommandFlowEngine.Core.Abstractions;

public interface ICommand<TRequest, TResponse>
where TRequest: IRequest
{
    Queue<IWorkflow<TRequest, TResponse>> Workflows { get; set; }

    public Task<TResponse> ExecuteAsync(TRequest request);
}