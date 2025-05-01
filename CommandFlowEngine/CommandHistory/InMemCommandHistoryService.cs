namespace CommandFlowEngine.CommandHistory;

public class InMemCommandHistoryService<TKey> : ICommandHistoryService<TKey>
    where TKey : notnull
{
    private readonly Dictionary<TKey, CommandMetadata> _commandHistory = new();

    public void AddCommandToHistory(TKey userId, Type type)
    {
        const int initialPosition = 0;
        
        var isAdded = _commandHistory.TryAdd(userId, new CommandMetadata(initialPosition, type));

        if (!isAdded)
        {
            _commandHistory[userId] = new CommandMetadata(initialPosition, type);
        }
    }

    public CommandMetadata? GetCommandFromHistory(TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var commandMetadata);

        return isExists ? commandMetadata : null;
    }

    public void MoveForward(TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);
        if (!isExists) return;
        command!.Position++;
        _commandHistory[userId] = command;
    }

    public void RemoveCommandFromHistory(TKey userId)
    {
        _commandHistory.Remove(userId);
    }
}