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

        private AsyncDuplexStreamingCall<CallRequest, CallResponse> RemoteCall { get; set; }
        public virtual void RemoteTask(string[] args) {}

        private void Init(string host, int port)
        {
            this.Host = host;
            this.Port = port;

            this.channel = new Channel(this.Host, this.Port, ChannelCredentials.Insecure);
            FileClient = new File.FileClient(channel);
            DirectoryClient = new Directory.DirectoryClient(channel);
            RemoteClient = new Remote.RemoteClient(channel);

            RemoteCall = RemoteClient.Call();

            AwaitRemote();
        }

        public void AwaitRemote()
        {
            var responseReaderTask = Task.Run(async () =>
            {
                while (await RemoteCall.ResponseStream.MoveNext())
                {
                    var response = RemoteCall.ResponseStream.Current;
                    RemoteTask(response.Args.Split(" "));
                }
            });
        }

        public async Task RegisterRemote()
        {
            await RemoteCall.RequestStream.WriteAsync(new CallRequest() { SessionId = Session.ToString() });
        }

        public async Task CallRemote(string args)
        {
            await RemoteCall.RequestStream.WriteAsync(new CallRequest() { SessionId = Session.ToString(), Args = args });
        }

        public void Close()
        {
            channel.ShutdownAsync().Wait();
        }
    }
}