﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.UnitTests.Tasks
{
    public class StandardTask : IConfigurationBootstrapTask
    {
        public Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)
        {
            throw new NotImplementedException();
        }
    }
}