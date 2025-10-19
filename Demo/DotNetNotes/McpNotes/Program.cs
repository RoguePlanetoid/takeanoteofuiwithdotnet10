var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Services
.RegisterServices()
.AddMcpServer()
.WithStdioServerTransport()
.WithTools<NotesTools>();
await builder.Build().RunAsync();