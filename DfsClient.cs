using System;
using System.Collections.Generic;
using Grpc.Core;
using GIO;
using Dfs.Impl;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dfs
{
    public class DfsClient
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public Guid Session { get; private set; }

        public File.FileClient FileClient { get; set; }
        public Directory.DirectoryClient DirectoryClient { get; set; }
        public Remote.RemoteClient RemoteClient { get; set; }
        private Channel channel;

        public DfsClient(string file = "")
        {
            Session = Guid.NewGuid();
            if (string.IsNullOrEmpty(file))
                file = "dfs.json";

            var json = System.IO.File.ReadAllText(file);
            var config = JsonConvert.DeserializeObject<RemoteConfig>(json);

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
            RemoteClient = new Remote.RemoteClient(channel);
        }

        public void AwaitCommands()
        {
            using (var call = RemoteClient.Call())
            {
                var responseReaderTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        var response = call.ResponseStream.Current;
                        Console.WriteLine("Received " + response.Method);
                    }
                });
                
                call.RequestStream.WriteAsync(new CallRequest() { Method = "test" });
            }
        }

        public void Close()
        {
            channel.ShutdownAsync().Wait();
        }
    }
}