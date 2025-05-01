namespace CommandFlowEngine.TypeResolution;

public interface ICommandTypeResolver
{
    public Type GetCommandType(string commandName);

    public IEnumerable<Type> GetRelatedWorkflowsTypeList(Type commandType);

    public bool IsCommandExists(string commandName);
}