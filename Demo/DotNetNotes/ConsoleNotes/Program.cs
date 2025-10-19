await Host.CreateDefaultBuilder(args)
.ConfigureServices(services => services.RegisterServices(args))
.Build().Services.GetRequiredService<ConsoleProvider>().Run();