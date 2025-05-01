using CommandFlowEngine.CommandHistory;
using CommandFlowEngine.Core;
using CommandFlowEngine.TypeResolution;
using Microsoft.Extensions.Logging;

namespace CommandFlowEngine.CommandExecutor;

public class CommandExecutor<TKey> : ICommandExecutor<TKey>
    where TKey : notnull
{
    private readonly ICommandHistoryService<TKey> _commandHistoryService;
    private readonly ICommandResolver _commandResolver;
    private readonly ILogger<ICommandExecutor<TKey>> _logger;

    public CommandExecutor(ICommandHistoryService<TKey> commandHistoryService,
        ICommandResolver commandResolver, ILogger<CommandExecutor<TKey>> logger)
    {
        _commandHistoryService = commandHistoryService;
        _commandResolver = commandResolver;
        _logger = logger;
    }

    public async Task<TResponse> ExecuteCommandAsync<TRequest, TResponse>(TRequest request, TKey userId)
        where TRequest : IRequest
    {
        TResponse response;
        var commandMetadataFromHistory = _commandHistoryService.GetCommandFromHistory(userId);
        var commandFromHistory = commandMetadataFromHistory is null
            ? null
            : _commandResolver.GetCommand<TRequest, TResponse>(
                commandMetadataFromHistory.CommandType,
                commandMetadataFromHistory.Position);

        var isExitCommand = _commandResolver.GetExitCommand<TRequest, TResponse>(request.Message);

        if (isExitCommand)
        {
            _commandHistoryService.RemoveCommandFromHistory(userId);
            commandFromHistory = null;
        }

        if (commandFromHistory == null)
        {
            var command = _commandResolver.GetCommand<TRequest, TResponse>(request.Message);

            response = await command.ExecuteAsync(request);

            if (command.Workflows.Count == 0) return response;

            _commandHistoryService.AddCommandToHistory(userId, command.GetType());
            return response;
        }

        var workflow = commandFromHistory.Workflows.Peek();

        _logger.LogInformation("Start executing workflow {Workflow}...", workflow.GetType());
        response = await workflow.ExecuteAsync(request);

        commandFromHistory.Workflows.Dequeue();
        _commandHistoryService.MoveForward(userId);

        if (commandFromHistory.Workflows.Count == 0)
        {
            _commandHistoryService.RemoveCommandFromHistory(userId);
        }

        return response;
    }
}