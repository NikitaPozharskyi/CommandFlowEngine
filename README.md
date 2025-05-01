# CommandFlowEngine

**CommandFlowEngine** is a flexible .NET library for orchestrating command-based workflows with pluggable state management and optional persistence.

It is designed to help developers manage asynchronous, non-sequential user requests in a clean and modular way. Whether you're building a chatbot, an interactive app, or a distributed service, this library gives you the tools to keep your flow logic consistent and testable.

---

## ✨ Features

- **Command & Workflow Interfaces:** Define modular, composable workflows using `ICommand` and `IWorkflow`.
- **Pluggable State Handling:** Use the built-in in-memory store or implement your own persistence layer (e.g., MongoDB) to support durable workflows.
- **Error-Resilient:** Designed to recover and continue workflows even after service restarts (with your custom state store).
- **Decoupled Design:** Keeps business logic separate from infrastructure concerns.
- **Lightweight & Extensible:** No unnecessary dependencies; easily integrates into existing projects.

---

## 🚀 Getting Started

### 1️⃣ Install

```bash
dotnet add package CommandFlowEngine
```
### 2️⃣ Define a Command
```bash
public class StartOrderCommand : ICommand
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
}
```

### 3️⃣ Define a Workflow
```bash
public class OrderWorkflow : IWorkflow
{
    public async Task HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        if (command is StartOrderCommand startOrder)
        {
            // Handle starting an order
            Console.WriteLine($"Starting order {startOrder.OrderId} for customer {startOrder.CustomerId}");
        }
    }
}
```

### 4️⃣ Setup DI
```bash
  services.AddCommandRegistry<long>(ServiceLifetime.Scoped);
  services
      .RegisterCommand<StartOrderCommand>("order", ServiceLifetime.Scoped)
      .RegisterWorkflow<OrderWorkflow>()
```

### 5️⃣ Wire It Up
```bash
var workflow = new OrderWorkflow();
var handler = new RequestHandler(workflow);

await handler.HandleAsync(new StartOrderCommand { OrderId = "123", CustomerId = "456" }, CancellationToken.None);
```

## 🧩 Extending Persistence
By default, the library uses in-memory state, but you can plug in your own persistence layer (e.g., MongoDB, Redis, SQL) to make workflows durable across restarts.

Example of your own IStateStore implementation (interface not included by default):

```bash
public class MongoStateStore : IStateStore
{
    // Implement saving and loading state from MongoDB
}
```

## 💡 Why Use This?
- Simplifies the orchestration of multi-step workflows
- Enables pluggable state management (volatile or persistent)
- Keeps your architecture clean and testable
- Ideal for chatbots, transactional services, and stateful APIs

## 🔧 Roadmap
- Built-in support for persistence adapters (MongoDB, SQL, etc.)
- Enhanced error handling & retries
- NuGet packaging
- Sample projects

## 🤝 Contributing
PRs and ideas are welcome! Feel free to open an issue or submit a pull request.

#.NET #C# #workflow #command pattern #stateful processing #request handler #asynchronous workflows #chatbot framework #middleware #orchestration