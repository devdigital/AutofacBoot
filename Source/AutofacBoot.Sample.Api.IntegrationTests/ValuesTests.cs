
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacBoot.Sample.Domain;
using AutofacBoot.Test;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    public class ValuesTests
    {
        [Theory]
        [AutoData]
        public async Task ValuesReturnsExpectedValues(
            ServerFactory serverFactory,
            Mock<IValuesRepository> valuesRepository,
            List<int> values)
        {
            valuesRepository.Setup(r => r.GetValues()).Returns
                (Task.FromResult(values.AsEnumerable()));

            using (var server = serverFactory
                .With<IValuesRepository>(valuesRepository.Object)
                .Create())
            {
                using (var client = server.CreateClient())
                {
                    var response = await client.GetAsync("api/values");
                    var responseValues = await response.To<IEnumerable<int>>();
                    Assert.Equal(values, responseValues);
                }
            }
        } 
    }
}
