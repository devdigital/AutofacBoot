using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AutofacBoot.Test
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<TData> To<TData>(
            this HttpResponseMessage response,
            TData defaultValue)
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

        public static async Task<IEnumerable<TData>> ToCollection<TData>(
            this HttpResponseMessage response)
        {
            return await To(response, Enumerable.Empty<TData>());
        }
    }
}