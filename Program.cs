using System;
using System.Linq;
using System.Collections.Generic;

namespace DotNetPlayground
{
    public static class FuncExtensions
    {
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(
            this Func<T1, T2, TResult> function) 
                => a => b => function(a, b);

        public static Func<T1, Action<T2>> Curry<T1, T2>(
            this Action<T1, T2> action) 
                => a => b => action(a, b);
        
        public static Func<T2, TResult> Partial<T1, T2, TResult>(
            this Func<T1, T2, TResult> function, T1 value)
                => value2 => function(value, value2);

        public static Func<T1, TResult> ReversedPartial<T1, T2, TResult>(
            this Func<T1, T2, TResult> function, T2 value2)
                => value => function(value, value2);

        public static Func<T2, T3, TResult> Partial<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, TResult> function, T1 value)
                => (value2, value3) => function(value, value2, value3);

        public static Func<IEnumerable<TSource>, IEnumerable<TSource>> Skip<TSource>(
            int count) 
                => source => Enumerable.Skip(source, count);

    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var max100 = new Func<int, int, int>(Math.Max).Partial(100);
            var skip3objects = new Func<IEnumerable<object>, int, IEnumerable<object>>(
                Enumerable.Skip).ReversedPartial(3);

            skip3objects(new object[] { 1, 2, 3, 4, 5});

            // No way to create lambda with generics. Is this a syntax only limitation?
            // var skip3 = new Func<IEnumerable<T>, int, IEnumerable<T>>(
            //     Enumerable.Skip<T>).ReversedPartial(3);


        }
    }
}
