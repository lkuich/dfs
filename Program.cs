using System;
using System.Collections.Generic;
using Grpc.Core;
using GIO;
using Dfs.Impl;
using System.Threading.Tasks;

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

                // client.Call()
                
                Console.WriteLine($"Started DFS client at: {DIO.Client.Host}:{DIO.Client.Port}");
                // var test = Console.ReadLine();
                

                //using (var c = client.RemoteClient.Call()) {
                //    (c.RequestStream.WriteAsync(new CallRequest() { Method = "test" })).Wait();
                //}
            
                // client.RemoteClient.SingleCall(new CallRequest() { Method = "test" });
                using (var call = client.RemoteClient.Call())
                {
                    
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var response = call.ResponseStream.Current;
                            Console.WriteLine("Received " + response.Method);
                        }
                    });
                    
                    Console.ReadLine();
                    call.RequestStream.WriteAsync(new CallRequest() { Method = "c" });
                    while(true) {
                        Console.ReadLine();
                        call.RequestStream.WriteAsync(new CallRequest() { Method = "test" });
                    }
                }


            }
        }
    }
}