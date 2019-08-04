using System;

namespace DotNetPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Program().Run();
        }
        delegate void PrintOperationDelegate<T1, T2>(T1 a, T2 b);

        PrintOperationDelegate<int, int> IntegerOperation;

        private void PrintIntegerSum(int a, int b) 
            => Console.WriteLine($"{a}+{b}={a + b}");
        
        private void PrintIntegerMultiplication(int a, int b) 
            => Console.WriteLine($"{a}*{b}={a * b}");

        private void Run()
        {
            IntegerOperation += new PrintOperationDelegate<int, int>(PrintIntegerSum);
            IntegerOperation += PrintIntegerMultiplication;
            IntegerOperation?.Invoke(3, 3);
        }

    }
}
