# AutofacBoot

Simple bootstrapping for ASP.NET Core applications running Autofac

```
install-package AutofacBoot
```

## Overview

Placing all of your application bootstrapping tasks into a single `Startup` file violates [SRP](https://en.wikipedia.org/wiki/Single_responsibility_principle). AutofacBoot allows you to define each bootstrapping task separately, keeping bootstrapping easier to write and maintain.

In an ASP.NET Core 2.0 application, your `Program.cs` and `Startup.cs` may look like this:

```
// Program.cs
public class Program
{   
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}

public class Startup
{
    public Startup(IHostingEnvironment environment)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

        this.Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {    
        services.AddMvc();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        // Autofac container registrations...
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
        loggerFactory.AddDebug();

        app.UseMvc();
    }
}
```

With AutofacBoot, you don't define a `Startup` type, and your `Program.cs` becomes:

```
public class Program
{
    public static void Main(string[] args)
    {
        new AutofacBootstrapper().Run();
    }
}
```

Your bootstrapping configuration then becomes separate tasks, for example:

Configuration:

```
public class ConfigurationBootstrapTask : IConfigurationBootstrapTask
{
    public Task Execute(ConfigurationBuilder configurationBuilder, IHostingEnvironment environment)
    {
        configurationBuilder
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

        return Task.CompletedTask;
    }
}
```

Services:

```
public class ServiceBootstrapTask : IServiceBootstrapTask
{
    public Task Execute(IServiceCollection services)
    {
        services.AddMvc().AddApplicationPart(
            typeof(MyController).Assembly);

        return Task.CompletedTask;
    }
}
```

> Note if you are using the MVC service, you need to specify where your controllers are located by using `AddApplicationPart` and passing the assembly where your controllers reside.

Autofac container configuration:

```
public class ContainerBootstrapTask : IContainerBootstrapTask
{
    public Task Execute(ContainerBuilder builder)
    {
        // access builder.RegisterType etc here...
        return Task.CompletedTask;
    }
}
```

Application configuration:

```
public class ApplicationBootstrapTask : IApplicationBootstrapTask
{
    private readonly ILoggerFactory loggerFactory;

    private readonly IConfigurationRoot configuration;

    public ApplicationBootstrapTask(
        ILoggerFactory loggerFactory,
        IConfigurationRoot configuration)
    {
        this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public Task Execute(IApplicationBuilder app)
    {
        this.loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
        this.loggerFactory.AddDebug();

        app.UseMvc();

        return Task.CompletedTask;
    }
}
```

> Note that any `IApplicationBootstrapTask` can have services injected via the constructor that have previously been registered in a service or Autofac container bootstrap task.

TODO: task type table

## Recipes

### Serilog

You likely want your logging configuration to run early in the bootstrap pipeline, therefore you can use `IOrderedTask` and apply a low value (here, -10):

```csharp
public class LoggingBootstrapTask : IApplicationBootstrapTask, IOrderedTask
{
    public int Order { get; } = -10;

    public Task Execute(IApplicationBuilder app)
    {
        Log.Logger = ...

        return Task.CompletedTask;
    }
}
```
