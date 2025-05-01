using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;
using CommandFlowEngine.TestApplication.Models;

namespace CommandFlowEngine.TestApplication;

public class ExitCommand : IPermanentExitCommand<MyRequest, MyResponse>
{
    public Queue<IWorkflow<MyRequest, MyResponse>> Workflows { get; set; }
    
    public Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        return Task.FromResult(new MyResponse
        {
            Message = "Exit command executed"
        });
    }
}