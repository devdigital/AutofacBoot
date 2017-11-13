using System;
using AutofacBoot.UnitTests.Services;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace AutofacBoot.UnitTests.Tests
{
    public class BootstrapperTests
    {
        [Theory]
        [AutoData]
        public void ExceptionWithoutHandlerThrowsException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration());

            Assert.Throws<NotImplementedException>(() => bootstrapper.Build());
        }

        [Theory]
        [AutoData]
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

        [Theory]
        [AutoData]
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

        [Theory]
        [AutoData]
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

        [Theory]
        [AutoData]
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

        [Theory]
        [AutoData]
        public void ExceptionWhenHandledReturnsNullHost()
        {
            var host = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => true)
                .Configure()
                .Build();

            Assert.Null(host);
        }

        [Theory]
        [AutoData]
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
