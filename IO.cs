using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Dfs.IO
{
    public class FileImpl : GIO.File.FileBase
    {
        public override async Task<GIO.WriteResponse> WriteAllBytes(GIO.WriteRequest request, ServerCallContext context)
        {
            Console.WriteLine("Writing to: " + request.Path);
            
            try {
                System.IO.File.WriteAllBytes(request.Path, request.Bytes.ToByteArray());
            } catch (Exception e) {
                return new GIO.WriteResponse() { Message = e.Message, Success = false };
            }

            return new GIO.WriteResponse() { Success = true };
        }

        public override async Task<GIO.ReadResponse> ReadAllBytes(GIO.ReadRequest request, ServerCallContext context)
        {
            Console.WriteLine("Reading from: " + request.Path);
            
            try {
                var bytes = Google.Protobuf.ByteString.CopyFrom(
                    System.IO.File.ReadAllBytes(request.Path)
                );
                return new GIO.ReadResponse() { Bytes = bytes, Success = true };
            } catch (Exception e) {
                return new GIO.ReadResponse() { Message = e.Message, Success = false };    
            }
        }
    }
}
