namespace CommandFlowEngine.Settings;

public class EngineSettings
{
    public Dictionary<string, Type> CommandDictionary { get; } = new();

    public Dictionary<Type, List<Type>> WorkflowDictionary { get; } = new();
}