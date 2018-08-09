// <copyright file="HttpResponseMessageExtensions.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// HTTP response message extensions.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Deserializes JSON to the specified type.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="response">The response.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Instance of the specified type.</returns>
        public static async Task<TData> FromJson<TData>(
            this HttpResponseMessage response,
            TData defaultValue = default(TData))
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TData>(content);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Deserializes JSON to the specified collection type.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>Collection of the specified type.</returns>
        public static async Task<IEnumerable<TData>> FromJsonCollection<TData>(
            this HttpResponseMessage response)
        {
            return await FromJson(response, Enumerable.Empty<TData>());
        }
    }
}