namespace CommandFlowEngine.Core;

public interface IWorkflow <in TRequest, TResponse>
where TRequest: IRequest
{
    Task<TResponse> ExecuteAsync(TRequest message);
}