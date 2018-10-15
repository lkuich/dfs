using System;

namespace Dirt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "server") {
                var dfs = new DfsServer();
                dfs.Start();

                Console.WriteLine("Started server...");
                Console.ReadLine();
            } else {
                

                Console.WriteLine("Started client...");
                Console.ReadLine();
            }
        }
    }
}
