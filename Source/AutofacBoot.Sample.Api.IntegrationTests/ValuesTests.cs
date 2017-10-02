
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacBoot.Sample.Domain;
using AutofacBoot.Test;
using Moq;
using Xunit;

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    public class ValuesTests
    {
        [Fact]
        public async Task ValuesReturnsExpectedValues()
        {
            var serverFactory = new ServerFactory();
            var valuesRepository = new Mock<IValuesRepository>();
            var values = new List<int> { 1, 2 };

            valuesRepository.Setup(r => r.GetValues()).Returns
                (Task.FromResult(values.AsEnumerable()));

            using (var server = serverFactory
                .With<IValuesRepository>(valuesRepository.Object)
                .Create())
            {
                using (var client = server.CreateClient())
                {
                    var response = await client.GetAsync("api/values");
                    var responseValues = await response.ToCollection<int>();
                    Assert.Equal(values, responseValues);
                }
            }
        } 
    }
}
