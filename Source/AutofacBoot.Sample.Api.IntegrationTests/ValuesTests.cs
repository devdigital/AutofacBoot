// <copyright file="ValuesTests.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutofacBoot.Sample.Domain;
    using AutofacBoot.Test;
    using AutoFixture.Xunit2;
    using Moq;
    using Xunit;

    #pragma warning disable SA1600
    #pragma warning disable 1591

    public class ValuesTests
    {
        [Theory]
        [AutoData]
        public async Task AdditionalConfigurationBootstrapTaskIsInvoked(ServerFactory serverFactory)
        {
            using (await serverFactory.Create())
            {
                Assert.True(serverFactory.ConfigurationInvoked);
            }
        }

        [Theory]
        [AutoData]
        public async Task ValuesReturnsExpectedValues(
            ServerFactory serverFactory,
            Mock<IValuesRepository> valuesRepository,
            List<int> values)
        {
            valuesRepository.Setup(r => r.GetValues()).Returns(Task.FromResult(values.AsEnumerable()));

            using (var server = await serverFactory
                .With<IValuesRepository>(valuesRepository.Object)
                .Create())
            {
                using (var client = server.CreateClient())
                {
                    var response = await client.GetAsync("api/values");
                    var responseValues = await response.FromJson<IEnumerable<int>>();
                    Assert.Equal(values, responseValues);
                }
            }
        }
    }
}