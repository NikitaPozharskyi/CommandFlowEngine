using CommandFlowEngine.CommandExecutor;
using CommandFlowEngine.CommandHistory;
using CommandFlowEngine.Extensions;
using CommandFlowEngine.TypeResolution;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CommandFlowEngine.Tests.CommandExecutor;

public class Tests
{
    private CommandExecutor<short> _commandExecutor;
    private ILogger<CommandExecutor<short>> _logger;
    private ICommandResolver _commandResolver;
    private ICommandHistoryService<short> _commandHistoryService;

    [SetUp]
    public void Setup()
    {
        _commandHistoryService = Substitute.For<ICommandHistoryService<short>>();
        _commandResolver = Substitute.For<ICommandResolver>();
        _logger = Substitute.For<ILogger<CommandExecutor<short>>>();

        _commandExecutor = new CommandExecutor<short>(_commandHistoryService, _commandResolver, _logger);
        _commandResolver.GetExitCommand<Request, Response>(Arg.Any<string>()).Returns(false);
    }

    [Test]
    public async Task ComplexTest()
    {
        short userId = 1;
        var firstCommand = new FirstCommand();
        var secondCommand = new SecondCommand();
        
        _commandResolver.GetCommand<Request, Response>("first").Returns(firstCommand);
        _commandResolver.GetCommand<Request, Response>(typeof(FirstCommand)).Returns(firstCommand);
        
        _commandResolver.GetCommand<Request, Response>("first", 1).Returns(new FirstCommand(firstCommand.Workflows.Skip(1).ToQueue()));
        _commandResolver.GetCommand<Request, Response>(typeof(FirstCommand), 1).Returns(new FirstCommand(firstCommand.Workflows.Skip(1).ToQueue()));
        
        _commandResolver.GetCommand<Request, Response>("second").Returns(secondCommand);
        _commandResolver.GetCommand<Request, Response>(typeof(SecondCommand)).Returns(secondCommand);
        
        _commandResolver.GetCommand<Request, Response>("second", 1).Returns(new SecondCommand(secondCommand.Workflows.Skip(1).ToQueue()));
        _commandResolver.GetCommand<Request, Response>(typeof(SecondCommand), 1).Returns(new SecondCommand(secondCommand.Workflows.Skip(1).ToQueue()));
        
        var response = await _commandExecutor.ExecuteCommandAsync<Request, Response>(new Request("first"), userId);
        
        Assert.That(response.Message, Is.EqualTo("first command executed"));
        
        // simulate registering command in history for user 1 and FirstCommand
        _commandHistoryService.GetCommandFromHistory(userId).Returns(new CommandMetadata(0, typeof(FirstCommand)));
        
        response = await _commandExecutor.ExecuteCommandAsync<Request, Response>(new Request("WF1"), userId);
        
        Assert.That(response.Message, Is.EqualTo("first workflow executed: WF1"));
        
        // simulate workflow shift for user 1 and FirstCommand
        _commandHistoryService.GetCommandFromHistory(userId).Returns(new CommandMetadata(1, typeof(FirstCommand)));
        
        response = await _commandExecutor.ExecuteCommandAsync<Request, Response>(new Request("WF2"), userId);
        
        Assert.That(response.Message, Is.EqualTo("second workflow executed: WF2"));
        
        
        // Second Command
        userId = 2;
        
        response = await _commandExecutor.ExecuteCommandAsync<Request, Response>(new Request("second"), userId);
        
        Assert.That(response.Message, Is.EqualTo("second command executed"));
        
        // simulate registering command in history for user 2 and SecondCommand
        _commandHistoryService.GetCommandFromHistory(userId).Returns(new CommandMetadata(0, typeof(SecondCommand)));
        
        response = await _commandExecutor.ExecuteCommandAsync<Request, Response>(new Request("WF2"), userId);
        
        Assert.That(response.Message, Is.EqualTo("second workflow executed: WF2"));
        
        // simulate workflow shift for user 2 and SecondCommand
        _commandHistoryService.GetCommandFromHistory(userId).Returns(new CommandMetadata(1, typeof(SecondCommand)));
        
        response = await _commandExecutor.ExecuteCommandAsync<Request, Response>(new Request("WF1"), userId);
        
        Assert.That(response.Message, Is.EqualTo("first workflow executed: WF1"));
    }
}