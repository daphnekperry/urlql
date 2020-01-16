using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using urlql.asp.core;
using urlql.asp.core.Filters;

namespace urlql.demo.asp.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        protected NorthwindContext nwContext { get; set; }

        public CustomersController(NorthwindContext context)
        {
            nwContext = context;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        [QueryArgumentsValidation]
        // Uncomment below to enforce paging on all requests with a default size of 25 and maximum size of 250
        // [QueryOptionsRequirePaging, QueryOptionsPageSize(25), QueryOptionsMaximumPageSize(50)]
        public async Task<IActionResult> Get(QueryArguments arguments, [FromServices] QueryOptions options)
        {
            var customers = nwContext.Customers;
            var resolver = new QueryResolver(customers, arguments ?? new QueryArguments(), options);
            return await resolver.GetIActionResultAsync();
        }

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return new OkObjectResult(await nwContext.Customers.Where(c => c.CustomerID == id).SingleOrDefaultAsync());
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            return new CreatedResult("/", value);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            return new OkObjectResult(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new OkResult();
        }
    }
}
