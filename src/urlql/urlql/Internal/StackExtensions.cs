using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Internal
{
    public static class StackExtensions
    {
        public static bool TryPop(this Stack<string> stack, out string value)
        {
            try
            {
                value = stack.Pop();
            }
            catch (InvalidOperationException)
            {
                value = null;
                return false;
            }
            return true;
        }
    }
}

