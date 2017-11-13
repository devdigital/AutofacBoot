using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacBoot.UnitTests.Tasks;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace AutofacBoot.UnitTests.Tests
{
    public class NumberedOrderedTaskResolverTests
    {
        [Theory]
        [AutoData]
        public async Task ReturnsOrderedTasks(NumberedTaskOrderer taskOrderer)
        {
            var tasks = new List<IConfigurationBootstrapTask>
            {
                new HighNumberedTask(),
                new StandardTask(),
                new LowNumberedTask()
            };

            var orderedTasks = await taskOrderer.Order(tasks);

            var expectedTasks = new List<IConfigurationBootstrapTask>
            {
                new LowNumberedTask(),
                new StandardTask(),
                new HighNumberedTask()
            };

            Assert.Equal(
                expectedTasks.Select(t => t.GetType()),
                orderedTasks.Select(t => t.GetType()));
        }
    }
}