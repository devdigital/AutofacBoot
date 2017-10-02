using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot.Sample.Domain
{
    public interface IValuesRepository
    {
        Task<IEnumerable<int>> GetValues();
    }
}
