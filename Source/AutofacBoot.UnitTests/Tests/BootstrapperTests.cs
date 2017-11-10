using System;
using System.Threading.Tasks;
using AutofacBoot.UnitTests.Services;
using Microsoft.AspNetCore.Hosting;
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

            Assert.Throws<AutofacBootException>(() => bootstrapper.Build());
        }

        [Theory]
        [AutoData]
        public void ExceptionWithHandlerPassesException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => Assert.NotNull(exception));

            bootstrapper.Build();
        }

        [Theory]
        [AutoData]
        public void ExceptionWithHandlerPassesLoggerFactory()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => Assert.NotNull(loggerFactory));

            bootstrapper.Build();
        }

        [Theory]
        [AutoData]
        public void ExceptionWithHandlerOnWebHostPassesException()
        {
            new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => Assert.NotNull(exception))
                .Configure()
                .Build();
        }

        [Theory]
        [AutoData]
        public void ExceptionWithHandlerOnWebHostPassesLoggerFactory()
        {
            new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => Assert.NotNull(loggerFactory))
                .Configure()
                .Build();
        }
    }
}
