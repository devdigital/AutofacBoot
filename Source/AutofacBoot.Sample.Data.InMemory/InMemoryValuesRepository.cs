// <copyright file="InMemoryValuesRepository.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Data.InMemory
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutofacBoot.Sample.Domain;

    /// <summary>
    /// In memory values repository.
    /// </summary>
    /// <seealso cref="AutofacBoot.Sample.Domain.IValuesRepository" />
    public class InMemoryValuesRepository : IValuesRepository
    {
        /// <inheritdoc />
        public Task<IEnumerable<int>> GetValues()
        {
            return Task.FromResult(new[] { 1, 2, 3 }.AsEnumerable());
        }
    }
}
