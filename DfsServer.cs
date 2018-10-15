using System;
using Grpc.Core;
using GIO;
using Dfs.IO;

namespace Dfs
{
    public class DfsServer {
        public bool ServerStarted { get; set; }
        public string Host { get; set; }
        public int Port { get; private set; }
        private Server server;
        
        public DfsServer(string host = "localhost", int port = 50051)
        {
            this.Host = host;
            this.Port = port;
        }
        
        public void Start() {
            server = new Server
            {
                Services = {
                    File.BindService(new FileImpl())
                },
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
            };
            server.Start();
            ServerStarted = true;
        }

        public async void Stop()
        {
            if (ServerStarted)
            {
                try 
                {
                    // ...
                } catch (Exception e) { }
                await server.ShutdownAsync();
                ServerStarted = false;
            }
        }
    }
}