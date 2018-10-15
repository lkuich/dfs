using System;

namespace Dfs
{
    sealed class Program
    {
        private static void Main(string[] args)
        {
            string cmd = args[0].ToLower();
            if (cmd == "server") {
                var dfs = new DfsServer();
                dfs.Start();

                Console.WriteLine($"Started DFS server at: {dfs.Host}:{dfs.Port}");
                Console.ReadLine();
            } else if (cmd == "client") {
                var client = new DfsClient();
                DIO.Client = client;
                
                Console.WriteLine($"Started DFS client at: {DIO.Client.Host}:{DIO.Client.Port}");

                var e = DIO.File.Exists("/home/aod/Desktop/test.txt");
                Console.WriteLine(e);
            }
        }
    }
}