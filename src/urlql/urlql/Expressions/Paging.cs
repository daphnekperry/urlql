using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    public class Paging
    {
        /// <summary>
        /// Where to start (skip to) when retreving a page
        /// </summary>
        public int Skip { get; protected set; }

        /// <summary>
        /// How many objects/records to retrieve for a page
        /// </summary>
        public int Take { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        public Paging(int skip, int take)
        {
            if(skip < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip), skip, $"skip: invalid value of {skip}");
            }

            if (take < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(take), take, $"take: invalid value of {take}");
            }

            Skip = skip;
            Take = take;
        }
    }
}
