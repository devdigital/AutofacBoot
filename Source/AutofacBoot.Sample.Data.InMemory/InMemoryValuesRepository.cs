using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacBoot.Sample.Domain;

namespace AutofacBoot.Sample.Data.InMemory
{
    public class InMemoryValuesRepository : IValuesRepository
    {
        public Task<IEnumerable<int>> GetValues()
        {
            return Task.FromResult(new[] {1, 2, 3}.AsEnumerable());
        }
    }
}
