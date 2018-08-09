// <copyright file="IOrderedTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    /// <summary>
    /// Ordered task.
    /// </summary>
    public interface IOrderedTask
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; }
    }
}