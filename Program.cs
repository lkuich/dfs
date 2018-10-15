using System;

namespace Dfs
{
    sealed class Program
    {
        private static void Main(string[] args)
        {
            string cmd = args[0].ToLower();
            if (cmd == "server") {
                string host = "localhost";
                int port = 50051;
                if (args.Length >= 2) {
                    host = args[1];
                    port = int.Parse(args[2]);
                }

                var dfs = new DfsServer(host, port);
                dfs.Start();

                Console.WriteLine($"Started DFS server at: {host}:{port}");
                Console.ReadLine();
            } else if (cmd == "client") {
                string host = "localhost";
                int port = 50051;
                if (args.Length >= 2) {
                    host = args[1];
                    port = int.Parse(args[2]);
                }
                Console.WriteLine($"Started DFS client at: {host}:{port}");

                var client = new DfsClient();
                DIO.Client = client;

                var e = DIO.File.Exists("/home/aod/Desktop/test.txt");
                Console.WriteLine(e);
            }
        }
    }
}