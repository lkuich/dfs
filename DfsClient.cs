using System;
using Grpc.Core;
using GIO;
using Dfs.IO;

namespace Dfs
{
    public class DfsClient
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        private File.FileClient FileClient { get; set; }
        private Channel channel;

        public DfsClient(string host = "localhost", int port = 50051)
        {
            this.Host = host;
            this.Port = port;

            this.channel = new Channel(this.Host, this.Port, ChannelCredentials.Insecure);
            FileClient = new File.FileClient(channel);
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            FileClient.WriteAllBytes(new WriteRequest() { Path = path, Bytes = Google.Protobuf.ByteString.CopyFrom(bytes) });
        }

        public byte[] ReadAllBytes(string path)
        {
            return FileClient.ReadAllBytes(new ReadRequest() { Path = path }).Bytes.ToByteArray();
        }

        public void Close()
        {
            channel.ShutdownAsync().Wait();
        }
    }
}