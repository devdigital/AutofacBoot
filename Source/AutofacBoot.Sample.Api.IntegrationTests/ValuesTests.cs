
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacBoot.Sample.Domain;
using Moq;
using Xunit;

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    public class ValuesTests
    {
        [Fact]
        public void ValuesReturnsExpectedValues()
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
                    var response = client.GetAsync("api/values");
                    Assert.NotNull(response);
                }
            }
        } 
    }
}
