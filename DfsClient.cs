using System;
using System.Collections.Generic;
using Grpc.Core;
using GIO;
using Dfs.Impl;
using Newtonsoft.Json;

namespace Dfs
{
    class ClientConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class DfsClient
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        public File.FileClient FileClient { get; set; }
        public Directory.DirectoryClient DirectoryClient { get; set; }
        private Channel channel;

        public DfsClient(string file = "")
        {
            if (string.IsNullOrEmpty(file))
                file = "dfs.json";

            var json = System.IO.File.ReadAllText(file);
            var config = JsonConvert.DeserializeObject<ClientConfig>(json);

            Init(config.Host, config.Port);        
        }

        public DfsClient(string host, int port)
        {
            Init(host, port);
        }

        private void Init(string host, int port)
        {
            this.Host = host;
            this.Port = port;

            this.channel = new Channel(this.Host, this.Port, ChannelCredentials.Insecure);
            FileClient = new File.FileClient(channel);
            DirectoryClient = new Directory.DirectoryClient(channel);
        }

        public void Close()
        {
            channel.ShutdownAsync().Wait();
        }
    }
}