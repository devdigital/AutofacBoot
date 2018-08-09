// <copyright file="IValuesRepository.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Values repository.
    /// </summary>
    public interface IValuesRepository
    {
        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns>The values.</returns>
        Task<IEnumerable<int>> GetValues();
    }
}
