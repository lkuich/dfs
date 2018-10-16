using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GIO;

namespace Dfs.Impl
{
    public class RemoteImpl : GIO.Remote.RemoteBase
    {
        public List<IAsyncStreamReader<CallRequest>> Requests { get; private set; }
        public List<IServerStreamWriter<CallResponse>> Responses { get; private set; }
        public RemoteImpl()
        {
            Requests = new List<IAsyncStreamReader<CallRequest>>();
        }
        public override async Task Call(IAsyncStreamReader<CallRequest> requestStream, IServerStreamWriter<CallResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine(Requests.Count);
            Requests.Add(requestStream);
            Responses.Add(responseStream);
            for (int i = 0; i < Requests.Count; i++) {
                var request = Requests.ElementAt(i);
                while (await request.MoveNext())
                {
                    var req = request.Current;
                    Console.WriteLine(req.Method);

                    await Responses.ElementAt(i).WriteAsync(new CallResponse() {
                        Method = req.Method,
                        Args = req.Args
                    });
                }
            }
        }
    }

    public class DirectoryImpl : Directory.DirectoryBase
    {
        public override async Task GetFiles(ReadRequest request, IServerStreamWriter<StringResponse> responseStream, ServerCallContext context)
        {
            var files = System.IO.Directory.GetFiles(request.Path);
            foreach (string file in files) {
                await responseStream.WriteAsync(new GIO.StringResponse() { Value = file });
            }
        }

        public override async Task GetDirectories(ReadRequest request, IServerStreamWriter<StringResponse> responseStream, ServerCallContext context)
        {
            var dirs = System.IO.Directory.GetDirectories(request.Path);
            foreach (string dir in dirs) {
                await responseStream.WriteAsync(new GIO.StringResponse() { Value = dir });
            }
        }

        public override async Task<ReadResponse> Exists(ReadRequest request, ServerCallContext context)
        {
            return new ReadResponse() { Success = System.IO.Directory.Exists(request.Path) };
        }
    }

    public class FileImpl : File.FileBase
    {
        public override async Task<WriteResponse> WriteAllBytes(WriteRequest request, ServerCallContext context)
        {
            try {
                System.IO.File.WriteAllBytes(request.Path, request.Bytes.ToByteArray());
                return new WriteResponse() { Success = true };
            } catch (Exception e) {
                return new WriteResponse() { Message = e.Message, Success = false };
            }
        }

        public override async Task<ReadResponse> ReadAllBytes(ReadRequest request, ServerCallContext context)
        {
            try {
                var bytes = Google.Protobuf.ByteString.CopyFrom(
                    System.IO.File.ReadAllBytes(request.Path)
                );
                return new ReadResponse() { Bytes = bytes, Success = true };
            } catch (Exception e) {
                return new ReadResponse() { Message = e.Message, Success = false };    
            }
        }
        public override async Task ReadAllLines(ReadRequest request, IServerStreamWriter<StringResponse> responseStream, ServerCallContext context)
        {
            var lines = System.IO.File.ReadAllLines(request.Path);
            foreach (string line in lines) {
                await responseStream.WriteAsync(new StringResponse() { Value = line });
            }
        }

        public override async Task<ReadResponse> Exists(ReadRequest request, ServerCallContext context)
        {
            return new ReadResponse() { Success = System.IO.File.Exists(request.Path) };
        }
    }
}
