using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace urlql.asp.core
{
    public static class QueryResolverExtensions
    {
        public static QueryResultViewModel GetResultsAsViewModel(this QueryResolver resolver)
        {
            return resolver.GetResults().AsViewModel();
        }

        public async static Task<QueryResultViewModel> GetResultsAsViewModelAsync(this QueryResolver resolver)
        {
            return (await resolver.GetResultsAsync()).AsViewModel();
        }

        /// <summary>
        /// Returns the result
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns>IActionResult of OkObjectResult containing the a QueryResultViewModel
        /// or IActionResult of BadRequestObjectResult containing an error message
        /// </returns>
        public static IActionResult GetOkObjectResult(this QueryResolver resolver)
        {
            try
            {
                return new OkObjectResult(resolver.GetResults().AsViewModel());
            }
            catch (QueryException qEx)
            {
                return new BadRequestObjectResult(qEx.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the result
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns>IActionResult of OkObjectResult containing the a QueryResultViewModel
        /// or IActionResult of BadRequestObjectResult containing an error message
        /// </returns>
        public async static Task<IActionResult> GetOkObjectResultAsync(this QueryResolver resolver)
        {
            try
            {
                return new OkObjectResult((await resolver.GetResultsAsync()).AsViewModel());
            }
            catch (QueryException qEx)
            {
                return new BadRequestObjectResult(qEx.Message);
            }
            catch
            {
                throw;
            }
        }
    }
}
