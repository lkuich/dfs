using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Dfs.Impl
{
    public class DirectoryImpl : GIO.Directory.DirectoryBase {
        public override async Task GetFiles(GIO.ReadRequest request, IServerStreamWriter<GIO.GetFilesResponse> responseStream, ServerCallContext context)
        {
            var files = System.IO.Directory.GetFiles(request.Path);
            foreach (string file in files) {
                await responseStream.WriteAsync(new GIO.GetFilesResponse() { File = file });
            }
        }
        public override async Task<GIO.ReadResponse> Exists(GIO.ReadRequest request, ServerCallContext context)
        {
            return new GIO.ReadResponse() { Success = System.IO.Directory.Exists(request.Path) };
        }
    }

    public class FileImpl : GIO.File.FileBase
    {
        public override async Task<GIO.WriteResponse> WriteAllBytes(GIO.WriteRequest request, ServerCallContext context)
        {
            try {
                System.IO.File.WriteAllBytes(request.Path, request.Bytes.ToByteArray());
                return new GIO.WriteResponse() { Success = true };
            } catch (Exception e) {
                return new GIO.WriteResponse() { Message = e.Message, Success = false };
            }
        }

        public override async Task<GIO.ReadResponse> ReadAllBytes(GIO.ReadRequest request, ServerCallContext context)
        {
            try {
                var bytes = Google.Protobuf.ByteString.CopyFrom(
                    System.IO.File.ReadAllBytes(request.Path)
                );
                return new GIO.ReadResponse() { Bytes = bytes, Success = true };
            } catch (Exception e) {
                return new GIO.ReadResponse() { Message = e.Message, Success = false };    
            }
        }

        public override async Task<GIO.ReadResponse> Exists(GIO.ReadRequest request, ServerCallContext context)
        {
            return new GIO.ReadResponse() { Success = System.IO.File.Exists(request.Path) };
        }
    }
}
