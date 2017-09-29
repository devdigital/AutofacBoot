//using System;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.TestHost;

//public abstract class TestServerFactory<TServerFactory>
//     where TServerFactory : TestServerFactory<TServerFactory>
//{
//    private readonly ITestStartup testStartup;

//    private Dictionary<Type, Type> TypeRegistrations { get; }

//    private Dictionary<Type, object> InstanceRegistrations { get; }

//    protected TestServerFactory(ITestStartup testStartup)
//    {
//        if (testStartup == null)
//        {
//            throw new ArgumentNullException(nameof(testStartup));
//        }

//        this.testStartup = testStartup;

//        this.TypeRegistrations = new Dictionary<Type, Type>();
//        this.InstanceRegistrations = new Dictionary<Type, object>();
//    }

//    public TServerFactory With<TInterface, TImplementation>()
//    {
//        if (this.TypeRegistrations.ContainsKey(typeof(TInterface)))
//        {
//            throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
//        }

//        this.TypeRegistrations[typeof(TInterface)] = typeof(TImplementation);
//        return this as TServerFactory;
//    }

//    public TServerFactory With<TInterface>(object instance)
//    {
//        if (instance == null)
//        {
//            throw new ArgumentNullException(nameof(instance));
//        }

//        if (this.InstanceRegistrations.ContainsKey(typeof(TInterface)))
//        {
//            throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
//        }

//        this.InstanceRegistrations[typeof(TInterface)] = instance;
//        return this as TServerFactory;
//    }

//    public virtual TestServer Create()
//    {
//        var registrations = new Registrations(this.TypeRegistrations, this.InstanceRegistrations);
//        return new TestServer()..Create(app => this.testStartup.Bootstrap(app, registrations));
//    }
//}