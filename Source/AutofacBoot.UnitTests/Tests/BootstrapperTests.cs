using System;
using System.Threading.Tasks;
using AutofacBoot.UnitTests.Services;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace AutofacBoot.UnitTests.Tests
{
    public class BootstrapperTests
    {
        [Theory]
        [AutoData]
        public async Task ExceptionWithoutHandlerThrowsException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration());

            await Assert.ThrowsAsync<AutofacBootException>(
                () => bootstrapper.RunAsync());
        }

        [Theory]
        [AutoData]
        public async Task ExceptionWithHandlerPassesException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => Assert.NotNull(exception));

            await bootstrapper.RunAsync();
        }

        [Theory]
        [AutoData]
        public async Task ExceptionWithHandlerPassesLoggerFactory()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler((exception, loggerFactory) => Assert.NotNull(loggerFactory));

            await bootstrapper.RunAsync();
        }
    }
}
