using System;
using System.Collections.Generic;
using Grpc.Core;
using GIO;
using Dfs.Impl;

namespace Dfs
{
    public static class IO
    {
        public static DfsClient Client { get; set; }

        public static class File
        {
            public static void WriteAllBytes(string path, byte[] bytes)
            {
                Client.FileClient.WriteAllBytes(new WriteRequest() { Path = path, Bytes = Google.Protobuf.ByteString.CopyFrom(bytes) });
            }

            public static byte[] ReadAllBytes(string path)
            {
                return Client.FileClient.ReadAllBytes(new ReadRequest() { Path = path }).Bytes.ToByteArray();
            }

            public static bool Exists(string path)
            {
                return Client.FileClient.Exists(new ReadRequest() { Path = path }).Success;
            }
        }

        public static class Directory
        {
            public static string[] GetFiles(string path)
            {
                var files = new List<string>();
                using (var request = Client.DirectoryClient.GetFiles(new ReadRequest() { Path = path }))
                {
                    while (request.ResponseStream.MoveNext().Result)
                    {
                        var response = request.ResponseStream.Current;
                        files.Add(response.File);
                    }
                }

                return files.ToArray();
            }

            public static bool Exists(string path)
            {
                return Client.DirectoryClient.Exists(new ReadRequest() { Path = path }).Success;
            }
        }
    }
}