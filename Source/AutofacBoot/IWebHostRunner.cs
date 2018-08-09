// <copyright file="IWebHostRunner.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;

    /// <summary>
    /// Web host runner.
    /// </summary>
    public interface IWebHostRunner
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        void Run();

        /// <summary>
        /// Runs the asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        Task RunAsync();
    }
}