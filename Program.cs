using System;
using System.Collections.Generic;
using Grpc.Core;
using GIO;
using Dfs.Impl;
using System.Threading.Tasks;

namespace Dfs
{
    public class MyDfsClient : DfsClient
    {
        public override void RemoteTask(string[] args)
        {
            foreach(var arg in args)
            {
                Console.WriteLine("Recieved: " + arg);
            }
        }
    }

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
                var client = new MyDfsClient();
                DIO.Client = client;

                Console.WriteLine($"Started DFS client at: {DIO.Client.Host}:{DIO.Client.Port}");
                while (true) {
                    var a = Console.ReadLine();
                    if (a.ToLower() == "remote") {
                       DIO.Client.RegisterRemote().Wait(); 
                    }
                    DIO.Client.CallRemote(a).Wait();
                }
            }
        }
    }
}