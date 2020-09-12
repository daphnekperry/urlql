using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using urlql;
using urlql.asp.core;
using urlql.asp.core.Filters;
using urlql.demo.asp.core.Entities;

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
        // Enforce paging on all requests with a default size of 25 and maximum size of 50
        [QueryOptionsRequirePaging, QueryOptionsPageSize(25), QueryOptionsMaximumPageSize(50)]
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
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(MergePatch<Customer> newCustomer)
        {          
            // You can leverage MergePatch for object creation as well...
            var nullValues = new List<string>();
            nullValues.AddRange(newCustomer.GetType().GetFields().Where(f => f.GetValue(newCustomer) == null).Select(f => f.Name));
            nullValues.AddRange(newCustomer.GetType().GetProperties().Where(p => p.GetValue(newCustomer) == null).Select(p => p.Name));
            if (nullValues.Any())
            {
                string msg = "The following contain null values:";
                nullValues.ForEach(n => msg = new string(msg.Concat($" {n}").ToArray()));
                return new BadRequestObjectResult(msg);
            }

            // By turning it into a real object before or after your input validation
            var customer = newCustomer.ToObject<Customer>();
            nwContext.Customers.Add(customer);
            return new CreatedResult("/", customer);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] MergePatch<Customer> customerChanges)
        {
            var customer = await nwContext.Customers
                .Where(c => c.CustomerID == id)
                .SingleOrDefaultAsync();

            if (customer == null)
            {
                return new NotFoundResult();
            }

            customerChanges.Apply(customer);
            nwContext.SaveChanges();
            return new OkObjectResult(customer);
        }
        
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await nwContext.Customers
                .Where(c => c.CustomerID == id)
                .SingleOrDefaultAsync();

            if (customer == null)
            {
                return new NotFoundResult();
            }
            nwContext.Customers.Remove(customer);
            return new NoContentResult();
        }
    }
}
