using System;
using System.Threading.Tasks;
using AutofacBoot.Sample.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AutofacBoot.Sample.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IValuesRepository valuesRepository;

        public ValuesController(IValuesRepository valuesRepository)
        {
            this.valuesRepository = valuesRepository ?? throw new ArgumentNullException(nameof(valuesRepository));
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await this.valuesRepository.GetValues();
            return this.Ok(values);
        }
    }
}