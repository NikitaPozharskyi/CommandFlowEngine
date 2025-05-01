namespace CommandFlowEngine.Core.Abstractions;

public interface IPermanentExitCommand<TRequest, TResponse> : ICommand<TRequest, TResponse>
where TRequest: IRequest;