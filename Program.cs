using System;
using System.Linq;

namespace DotNetPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().CreateFunctions();
        }

        private void CreateFunctions()
        {
            // Squared value as string
            // "10" -> "20"
            // "-2" -> "4"
            string SquaredValueAsString1(string value) // "-5"
            {
                var valueAsInteger = Convert.ToInt32(value); // -5
                var squaredInteger = Math.Pow(valueAsInteger, 2); // 25 
                var squaredValueAsString = Convert.ToString(squaredInteger); // "25" 
                return squaredValueAsString;
            };
            string SquaredValueAsString2(string value) // "-5"
            {
                return 
                    Convert.ToString(
                        Math.Pow(
                            Convert.ToInt32(
                                value
                            ), 
                            2
                        )
                    );
            };
            string SquaredValueAsString3(string value) // "-5"
            {
                Func<string, int> stringAsInteger = Convert.ToInt32;
                Func<int, double> square = integer => Math.Pow(integer, 2);
                Func<double, string> doubleAsString = Convert.ToString;

                return doubleAsString(square(stringAsInteger(value)));
            };
            
            Func<string, int> strAsInt = Convert.ToInt32;
            Func<int, double> sqr = integer => Math.Pow(integer, 2);
            Func<double, string> doubleAsStr = Convert.ToString;
            
            Func<string, string> SquaredValueAsString
                = strAsInt.Then(sqr).Then(doubleAsStr);

            Console.WriteLine(SquaredValueAsString("7")); // "49"
            Func<int, double> byTwo = v => v / 2.0;

            Func<string, string> meuPipeLineMuchoLoco
             = strAsInt.Then(Math.Abs).Then(byTwo).Then(Convert.ToString);

            Console.WriteLine(meuPipeLineMuchoLoco("-3")); // "1.5"
        }
    }

    public static partial class FuncExtensions
    {
        public static Func<T, TResult2> Then<T, TResult1, TResult2>(
            this Func<T, TResult1> function1,
            Func<TResult1, TResult2> function2
        ) => value => function2(function1(value));

        public static Func<T, TResult1> After<T, TResult1, TResult2>(
            this Func<TResult2, TResult1> function1,
            Func<T, TResult2> function2
        ) => value => function1(function2(value));
    }
    public static partial class FuncExtensions
    {

        // Transform (T1, T2) -> TResult
        // to T1 -> T2 -> TResult.
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(
            this Func<T1, T2, TResult> function) => 
                value1 => value2 => function(value1, value2);
        
        public static Func<T2, TResult> Partial<T1, T2, TResult>(
            this Func<T1, T2, TResult> function, T1 value1) => 
                value2 => function(value1, value2);

        public static Func<T2, Func<T3, TResult>> Partial<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, TResult> function, T1 value1) => 
                value2 => value3 => function(value1, value2, value3);

        public static Func<T2, Func<T3, Func<T4, TResult>>> Partial<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, TResult> function, T1 value1) => 
                value2 => value3 => value4 => function(value1, value2, value3, value4);

        // ...
    }
}
