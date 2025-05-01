using CommandFlowEngine.Core.Abstractions;
using CommandFlowEngine.TestApplication.Models;

namespace CommandFlowEngine.TestApplication;

public class TestCommand : CommandAbstract<MyRequest, MyResponse>
{
    public override Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        Console.WriteLine("Executing command 1...");
    
        return Task.FromResult(new MyResponse
        {
            Message = "TestCommand Executed"
        });
    }
}