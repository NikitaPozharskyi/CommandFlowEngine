namespace CommandFlowEngine.CommandHistory;

public class CommandMetadata(int position, Type commandType)
{
    public int Position { get; set; } = position;
    
    public Type CommandType { get; } = commandType;
}