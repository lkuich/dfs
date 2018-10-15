using System;
using Grpc.Core;
using GIO;
using Dfs.Impl;
using Newtonsoft.Json;

namespace Dfs
{
    public class RemoteConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class DfsServer
    {
        public bool ServerStarted { get; set; }
        public string Host { get; set; }
        public int Port { get; private set; }
        private Server server;

        public DfsServer(string file = "")
        {
            if (string.IsNullOrEmpty(file))
                file = "dfs.json";

            var json = System.IO.File.ReadAllText(file);
            var config = JsonConvert.DeserializeObject<RemoteConfig>(json);

            Init(config.Host, config.Port);
        }
        
        public DfsServer(string host, int port)
        {
            Init(host, port);
        }

        public void Init(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }
        
        public void Start() {
            server = new Server
            {
                Services = {
                    Directory.BindService(new DirectoryImpl()),
                    File.BindService(new FileImpl()),
                    Remote.BindService(new RemoteImpl())
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