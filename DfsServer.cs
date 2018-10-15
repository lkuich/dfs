using System;
using Grpc.Core;
using GIO;
using Dfs.Impl;

namespace Dfs
{
    public class DfsServer
    {
        public bool ServerStarted { get; set; }
        public string Host { get; set; }
        public int Port { get; private set; }
        private Server server;
        
        public DfsServer(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }
        
        public void Start() {
            server = new Server
            {
                Services = {
                    Directory.BindService(new DirectoryImpl()),
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