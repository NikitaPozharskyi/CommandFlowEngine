using CommandFlowEngine.Core;

namespace CommandFlowEngine.CommandExecutor;

public interface ICommandExecutor<in TKey>
where TKey: notnull
{
    Task<TResponse> ExecuteCommandAsync<TRequest, TResponse>(TRequest request, TKey userId)
        where TRequest : IRequest;
}