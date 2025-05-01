using CommandFlowEngine.Exceptions;
using CommandFlowEngine.Settings;
using Microsoft.Extensions.Options;

namespace CommandFlowEngine.TypeResolution;

public class CommandTypeResolver : ICommandTypeResolver
{
    private readonly EngineSettings _engineSettings;
    
    public CommandTypeResolver(IOptions<EngineSettings> commandAndWorkflowSettings)
    {
        _engineSettings = commandAndWorkflowSettings.Value;
    }

    public Type GetCommandType(string commandName)
    {
        var isExists = _engineSettings.CommandDictionary.TryGetValue(commandName, out var messageType);

        if (!isExists)
        {
            throw new InvalidCommandTypeException($"There is no registered command with type {commandName}");
        }

        return messageType!;
    }

    public IEnumerable<Type> GetRelatedWorkflowsTypeList(Type commandType)
    {
        var isExists = _engineSettings.WorkflowDictionary.TryGetValue(commandType, out var workflows);
        
        return !isExists ? [] : workflows!;
    }

    public bool IsCommandExists(string commandName) =>
        _engineSettings.CommandDictionary.TryGetValue(commandName, out _);
}