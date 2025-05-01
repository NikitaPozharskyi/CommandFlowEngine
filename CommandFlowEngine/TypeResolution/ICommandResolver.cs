using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;

namespace CommandFlowEngine.TypeResolution;

public interface ICommandResolver
{
    ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(string commandName, int skip = 0)
        where TRequest : IRequest;

    ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType, int skip = 0)
        where TRequest : IRequest;

    bool GetExitCommand<TRequest, TResponse>(string commandName)
        where TRequest : IRequest;
}