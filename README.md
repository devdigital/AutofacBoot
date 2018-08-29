Simple bootstrapping for ASP.NET Core applications running Autofac

```
install-package AutofacBoot
```

# Application Bootstrapping

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

## Task Types

Tasks are run in the following order:

| Type          | Interface                     | Method                                                                                                         | Can Inject Services |
| ------------- | ----------------------------- | -------------------------------------------------------------------------------------------------------------- | ------------------------------- |
| Configuration | `IConfigurationBootstrapTask` | `Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)`                    | No                              |
| Services      | `IServiceBootstrapTask`       | `Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, IServiceCollection services)` | No                              |
| Container     | `IContainerBootstrapTask`     | `Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, ContainerBuilder builder)`    | No                              |
| Application   | `IApplicationBootstrapTask`   | `Task Execute(IApplicationBuilder app)`                                                                        | Yes                             |

## Conditional Execution

Service, Container, and Application tasks can be conditionally executed by adding the `IConditionalExecution` interface to the task type and implementing the `Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)` method:

```csharp
public class ApplicationWontExecuteTask : IApplicationBootstrapTask, IConditionalExecution
{
    public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
    {
        // Can check configuration and determine if this task is executed...
        return Task.FromResult(false);
    }
    
    public Task Execute(IApplicationBuilder app)
    {
        // This won't be executed
        return Task.CompletedTask;
    }
}
```

## Task Order

You can change the order in which tasks are executed by adding the `IOrderedTask` interface to the task type and implementing the `int Order { get; }` property. The lower the value, the earlier in the pipeline the task will be executed. By default, all tasks have an order value of 0.

```csharp
public class ApplicationLastBootstrapTask : IApplicationBootstrapTask, IOrderedTask
{
    public int Order => 100;
    
    public Task Execute(IApplicationBuilder app)
    {
        // This will execute after all other tasks with an order < 100
        return Task.CompletedTask;
    }
}
```

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

# Testing

```
install-package AutofacBoot.Test
```

For integration testing, it's useful to use the same bootstrapping process but have the ability to override service registrations for stubs and mocks.

The `AutofacBoot.Test` package provides a `TestServerFactory<TServerFactory>` abstract class which you can use to write integration tests using the same bootstrapping process. In its simplest form, create a server factory type deriving from `TestServerFactory<TServerFactory>` and implement the `Task<ITaskResolver> GetTaskResolver` method:

```csharp
public class MyServerFactory : TestServerFactory<MyServerFactory>
{
    protected override Task<ITaskResolver> GetTaskResolver()
    {
        ITaskResolver taskResolver = new AssemblyTaskResolver(
            typeof(MyBootstrapTask).Assembly);

        return Task.FromResult(taskResolver);
    }
}
```

In this instance, the task resolver used is a provided `AssemblyTaskResolver` which takes an assembly or collection of assemblies and scans them for task types. To use the same bootstrapping process in your tests as in production, specify the assembly that contains your production bootstrapping tasks.

You can then use the server factory within your tests:

```csharp
[Theory]
[AutoData]
public async Task Return200(MyServerFactory serverFactory)
{
    using (var server = await serverFactory.Create())
    {
        using (var client = server.CreateClient())
        {
            var response = await client.GetAsync("api/foo");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
```

> Note that AutoFixture is used here to allow the server factory to be injected into the test method, thus reducing any 'arrange' part of the test.

The server factory `Create` method returns a standard `TestServer` type provided by the `Microsoft.AspNetCore.TestHost` package.

## Replacing Service Registrations

To override service registrations in your test, you can use the builder methods provided by the `TestServerFactory`. These include `With<TInterface, TImplementation>` for type registrations, and `With<TInterface>(object instance)` for instance registrations.

> Note that the registrations made within the `TestServerFactory` are executed last in the pipeline, overwriting any registrations made in the production bootstrapping.

For example:

```csharp
[Theory]
[AutoData]
public async Task ValuesReturnsExpectedValues(
    MyServerFactory serverFactory,
    Mock<IValuesRepository> valuesRepository,
    List<int> values)
{
    valuesRepository.Setup(r => r.GetValues()).ReturnsAsync(values);

    using (var server = await serverFactory
        .With<IValuesRepository>(valuesRepository.Object)
        .Create())
    {
        using (var client = server.CreateClient())
        {
            var response = await client.GetAsync("api/values");
            var responseValues = await response.FromJsonCollection<int>();
            Assert.Equal(values, responseValues);
        }
    }
}
```

> Note that in this example Moq is used to test that the data returned from the repository is returned by the controller action. `AutofacBoot.Test` provides `HttpResponseMessage` extensions `FromJson<T>` and `FromJsonCollection<T>` for JSON deserialization. `response.FromJsonCollection<int>()` is equivalent to `response.FromJson<IEnumerable<int>>()`.

## Recipes

TODO
