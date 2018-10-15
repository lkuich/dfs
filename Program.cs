using System;

namespace Dfs
{
    class Program
    {
        static void Main(string[] args)
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
                Dfs.IO.Client = client;

                // var bytes = client.ReadAllBytes("/home/aod/Documents/dfs/test.txt");
                // client.WriteAllBytes("/home/aod/Desktop/test.txt", bytes);
                
                var res = Dfs.IO.Directory.GetFiles("/home/aod/Desktop");
                foreach(var r in res) {
                    Console.WriteLine(r);
                }
            }
        }
    }
}