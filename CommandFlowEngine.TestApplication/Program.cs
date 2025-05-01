using CommandFlowEngine.Extensions;
using CommandFlowEngine.TestApplication;
using CommandFlowEngine.TestApplication.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string testCommand = "Test Command";
const string testCommand2 = "Test Command2";
const string testCommand3 = "Test Command3";
const string exitCommand = "exit";

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCommandRegistry<long>(ServiceLifetime.Scoped);
        
        services.RegisterCommand<TestCommand3>(testCommand3, ServiceLifetime.Scoped);
        services.RegisterCommand<ExitCommand>(exitCommand, ServiceLifetime.Scoped);
        services.RegisterCommand<TestCommand2>(testCommand2, ServiceLifetime.Scoped);
        
        // TestCommand1 - TestWorkflow - TestWorkflow2 - TestWorkflow3. -> TestCommand1
        // TestCommand2
        // TestCommand3
        // TestCommand4
        // TestCommand5
        // TestCommand6
        
        services
            .RegisterCommand<TestCommand>(testCommand, ServiceLifetime.Scoped)
            .RegisterWorkflow<TestWorkflow>()
            .RegisterWorkflow<TestWorkflow2>();
        
        services.AddScoped<CustomCommandExecutor>();
    })
    .Build();

// TestCommand - TestWorkflow - TestWorkflow2

var commandExecutor = host.Services.GetRequiredService<CustomCommandExecutor>();

var result2 = await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = testCommand2
}, 1234);
Console.WriteLine(result2.Message);

var result3 = await commandExecutor.ExecuteCommandAsync<MyCustomRequest, MyCustomResponse>(new MyCustomRequest
{
    Message = testCommand3
}, 1234);

Console.WriteLine(result3.Message);

var result = await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = testCommand
}, 1234);
Console.WriteLine(result.Message);



// test in case if we have a workflows to execute.
await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = "Workflow message"
}, 1234);

// var exitResult = await commandExecutor.ExecuteCommandAsync(new MyRequest
// {
//     Message = exitCommand
// }, 1234);
//
// Console.WriteLine(exitResult.Message);

await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = "Workflow message 2"
}, 1234);

await host.StartAsync();
await host.StopAsync();
