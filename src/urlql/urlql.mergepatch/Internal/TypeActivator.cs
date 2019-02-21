using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace urlql.mergepatch
{
    public delegate object ObjectActivator(params object[] args);

    public static class TypeActivator
    {
        internal static ConcurrentDictionary<Guid, ObjectActivator> ActivatorCache = new ConcurrentDictionary<Guid, ObjectActivator>();

        public static ObjectActivator GetActivator(Type type)
        {
            var hasActivator = ActivatorCache.TryGetValue(type.GUID, out ObjectActivator activator);
            if (!hasActivator)
            {
                activator = CreateActivator(type);
                ActivatorCache.TryAdd(type.GUID, activator);
            }
            return activator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctorInfo"></param>
        /// <returns></returns>
        /// <remarks>Lovingly borrowed from Roger Johansson https://rogerjohansson.blog/2008/02/28/linq-expressions-creating-objects/ </remarks>
        internal static ObjectActivator CreateActivator(Type type)
        {
            var ctorInfo = type.GetConstructors().FirstOrDefault();
            ParameterInfo[] paramsInfo = ctorInfo.GetParameters();

            //create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctorInfo, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            ObjectActivator compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }
    }
}
