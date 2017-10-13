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

            await Assert.ThrowsAsync<NotImplementedException>(() => bootstrapper.RunAsync());
        }

        [Theory]
        [AutoData]
        public async Task ExceptionWithHandlerPassesException()
        {
            var bootstrapper = new AutofacBootstrapper()
                .WithContainer(new ExceptionThrowingContainerConfiguration())
                .WithExceptionHandler(Assert.NotNull);

            await bootstrapper.RunAsync();
        }
    }
}
