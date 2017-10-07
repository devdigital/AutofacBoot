﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.UnitTests.Tasks
{
    public class LowNumberedTask : IConfigurationBootstrapTask, IOrderedTask
    {
        public Task Execute(ConfigurationBuilder configurationBuilder, IHostingEnvironment environment)
        {
            throw new NotImplementedException();
        }

        public int Order => -10;
    }
}