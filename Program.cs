using System;

namespace Dfs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].ToLower() == "server") {
                var dfs = new DfsServer();
                dfs.Start();

                Console.WriteLine("Started server...");
                Console.ReadLine();
            } else {
                Console.WriteLine("Started client...");

                var client = new DfsClient();
                var bytes = client.ReadAllBytes("/home/aod/Documents/dfs/test.txt");
                client.WriteAllBytes("/home/aod/Desktop/test.txt", bytes);
            }
        }
    }
}