// <copyright file="ValuesController.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutofacBoot.Sample.Domain;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Values controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IValuesRepository valuesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesController"/> class.
        /// </summary>
        /// <param name="valuesRepository">The values repository.</param>
        public ValuesController(IValuesRepository valuesRepository)
        {
            this.valuesRepository = valuesRepository ?? throw new ArgumentNullException(nameof(valuesRepository));
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns>The values.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await this.valuesRepository.GetValues();
            return this.Ok(values);
        }
    }
}