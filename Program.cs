using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            new AsyncTests();
        }
    }

    public class AsyncTests
    {
        public AsyncTests()
        {
            System.Console.WriteLine("Running program...");
            Run();  
        }

        private void Run()
        {
            var printNamesTask = PrintNamesAsync();

            var updateUITask = 
                Task.Run(async () =>
                {
                    while (!printNamesTask.IsCompleted)
                    {
                        System.Console.Write(".");
                        await Task.Delay(250);
                    }
                });

            System.Console.WriteLine(
                "Doing other important things while " +
                "the names are being fetched...");

            Task.WaitAll(printNamesTask, updateUITask);
        }

        private async Task PrintNamesAsync()
        {
            System.Console.WriteLine("Getting names from database");
                        
            var names = await GetNames();
            
            System.Console.WriteLine("\nNames found:");
            System.Console.WriteLine(string.Join(", ", names));
        }

        private async Task<List<string>> GetNames()
        {
            await Task.Delay(3000);

            return new List<string> { "Marcel", "Marcelo", "Marcell"};
        }

    }

}
