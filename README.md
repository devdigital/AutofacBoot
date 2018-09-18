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
| ------------- | ----------------------------- | -------------------------------------------------------------------------------------------------------------- | ------------------- |
| Configuration | `IConfigurationBootstrapTask` | `Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)`                    | No                  |
| Services      | `IServiceBootstrapTask`       | `Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, IServiceCollection services)` | No                  |
| Container     | `IContainerBootstrapTask`     | `Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, ContainerBuilder builder)`    | No                  |
| Application   | `IApplicationBootstrapTask`   | `Task Execute(IApplicationBuilder app)`                                                                        | Yes                 |

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

## Reusing Service Registrations

If you have many tests that use the same service setup, the test server factory supports configuring registrations within a separate type:

```csharp
public class MyConfiguration : IServerFactoryConfiguration<MyServerFactory>
{
    public void Configure(MyServerFactory factory)
    {
        factory.With<IValuesRepository>(...);
        // etc.
    }
}
```

To use the configuration, use the `WithConfiguration` method:

```csharp
using (var server = await serverFactory
    .WithConfiguration(new MyConfiguration())
    .Create())
{
    ...
}
```

You can use the `WithConfigurations` method to register multiple configurations:

```csharp
using (var server = await serverFactory
    .WithConfigurations(new MyConfiguration(), new MyOtherConfiguration())
    .Create())
{
    ...
}
```

## Advanced Registrations

The `With` methods and configurations provide a mechanism for simple instance or type registrations, which are suitable in most use cases.

However, if you wish to have access to the full Autofac registration functionality within tests, you can use the `WithContainerConfiguration` method.

This takes an `IContainerConfiguration` which gives access to the Autofac `ContainerBuilder` and the current environment:

```csharp
public interface IContainerConfiguration
{
    Task Configure(
        IHostingEnvironment environment,
        ContainerBuilder builder);
}
```

You can implement this interface to perform any Autofac registrations you wish:

```csharp
public class MyContainerConfiguration : IContainerConfiguration
{
    public Task Configure(
        IHostingEnvironment environment,
        ContainerBuilder builder)
    {
        // Perform any Autofac registrations
        builder.RegisterGeneric(...)

        return Task.CompletedTask;
    }
}
```

Then use your container configuration within the server factory builder:

```csharp
using (var server = await serverFactory
    .WithContainerConfiguration(new MyContainerConfiguration())
    .Create())
{
    ...
}
```

## Running Test Middleware

There are occasions when you may need to run custom middleware within your tests.

First, implement `IAppBuilderConfiguration` which gives access to the ASP.NET Core `IApplicationBuilder`:

```csharp
public class MyAppBuilderConfiguration : IAppBuilderConfiguration
{
    public Task Configure(IApplicationBuilder app)
    {
        // Add middleware here

        return Task.CompletedTask;
    }
}
```

Then use your app builder configuration within the test server factory using the `WithAppBuilderConfiguration` method:

```csharp
using (var server = await serverFactory
    .WithAppBuilderConfiguration("id", new MyAppBuilderConfiguration())                
    .Create())
{
    ...
}
```

The `WithAppBuilderConfiguration` also takes an identifier string which is used within your bootstrapping code to get access to the middleware. This gives flexibility to where your middleware is invoked in the pipeline. 

To add your middleware, you can inject the provided `IAppBuilderConfigurationResolver` provided by the `AutofacBoot` package:

```csharp
public class ApplicationBootstrapTask : IApplicationBootstrapTask
{
    private readonly IAppBuilderConfigurationResolver configurationResolver;

    public ApplicationBootstrapTask(IAppBuilderConfigurationResolver configurationResolver)
    {        
        this.configurationResolver = configurationResolver
            ?? throw new ArgumentNullException(nameof(configurationResolver));
    }

    public Task Execute(IApplicationBuilder app)
    {
        var myMiddleware = this.configurationResolver.Resolve("id");
        myMiddleware.Configure(app);

        app.UseMvc();

        return Task.CompletedTask;
    }
}
```

The `IAppBuilderConfigurationResolver` provides a `Resolve` method which takes the same identifier string as registrered with the test server factory.

If the identifier doesn't exist (as would be the case when running the same bootstrapper in production), the `Resolve` method returns a `NullAppBuilderConfiguration` which does nothing, so you don't need to handle app builder configurations that are not registered.

If you do not wish to use string identifiers, you can also use the generic `WithAppBuilderConfiguration` and `Resolve` methods, which use the provided type's full name as the identifier:

```csharp
using (var server = await serverFactory
    .WithAppBuilderConfiguration<MyAppBuilderConfiguration>(
        new MyAppBuilderConfiguration())
    .Create())
{
    ...
}
```

> Note this can be simplified to `WithAppBuilderConfiguration(new MyAppBuilderConfiguration())`

Then, use the generic `Resolve` method within your production bootstrap code:

```csharp
var myMiddleware = this.configurationResolver.Resolve<MyAppBuilderConfiguration>();
myMiddleware.Configure(app);
```
