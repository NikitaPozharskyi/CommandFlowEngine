using CommandFlowEngine.Core.Abstractions;
using CommandFlowEngine.TestApplication.Models;

namespace CommandFlowEngine.TestApplication;

public class TestCommand2 : CommandAbstract<MyRequest, MyResponse>
{
    public override Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        return Task.FromResult(new MyResponse
        {
            Message = "TestCommand2 executed"
        });
    }
}