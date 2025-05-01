using CommandFlowEngine.Core;

namespace CommandFlowEngine.TestApplication.Models;

public class MyRequest : IRequest
{
    public string Format { get; set; }
    
    public string Message { get; set; }
}

public class MyResponse
{
    public string Message { get; set; }
}