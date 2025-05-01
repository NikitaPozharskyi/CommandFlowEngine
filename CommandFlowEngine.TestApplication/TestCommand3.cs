using CommandFlowEngine.Core;
using CommandFlowEngine.Core.Abstractions;

namespace CommandFlowEngine.TestApplication;

public class TestCommand3 : CommandAbstract<MyCustomRequest, MyCustomResponse>
{
    public override Task<MyCustomResponse> ExecuteAsync(MyCustomRequest request)
    {
        return Task.FromResult<MyCustomResponse>(new MyCustomResponse
        {
            Message = "TestCommand 3 Custom request/response"
        });
    }
}

public class MyCustomRequest : IRequest
{
    public string Message { get; set; }
}

public class MyCustomResponse : IResponse
{
    public string Message { get; set; }
}