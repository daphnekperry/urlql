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
    }
}
