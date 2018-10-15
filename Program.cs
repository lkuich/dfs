using System;

namespace Dirt
{
    class Program
    {
        static void Main(string[] args)
        {
            var dfs = new DfsServer();
            dfs.Start();

            Console.WriteLine("Started server...");
            Console.ReadLine();
        }
    }
}
