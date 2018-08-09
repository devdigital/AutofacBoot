// <copyright file="BootstrapperTests.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Tests
{
    using System;
    using AutofacBoot.UnitTests.Services;
    using Xunit;

    #pragma warning disable SA1600
    #pragma warning disable 1591

    public class BootstrapperTests
    {
        [Fact]
        public void ExceptionWithoutHandlerThrowsException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration());

            Assert.Throws<NotImplementedException>(() => bootstrapper.Build());
        }

        [Fact]
        public void ExceptionWithHandlerPassesException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) =>
                {
                    Assert.NotNull(exception);
                    return true;
                });

            bootstrapper.Build();
        }

        [Fact]
        public void ExceptionWithHandlerPassesLoggerFactory()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) =>
                {
                    Assert.NotNull(loggerFactory);
                    return true;
                });

            bootstrapper.Build();
        }

        [Fact]
        public void ExceptionWithHandlerOnWebHostPassesException()
        {
            new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) =>
                {
                    Assert.NotNull(exception);
                    return true;
                })
                .Configure()
                .Build();
        }

        [Fact]
        public void ExceptionWithHandlerOnWebHostPassesLoggerFactory()
        {
            new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) =>
                {
                    Assert.NotNull(loggerFactory);
                    return true;
                })
                .Configure()
                .Build();
        }

        [Fact]
        public void ExceptionWhenHandledReturnsNullHost()
        {
            var host = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => true)
                .Configure()
                .Build();

            Assert.Null(host);
        }

        [Fact]
        public void ExceptionWhenNotHandledThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => false)
                .Configure()
                .Build());
        }
    }
}
