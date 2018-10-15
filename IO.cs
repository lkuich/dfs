using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Dirt.IO
{
    public class FileImpl : GIO.File.FileBase
    {
        public override async Task<GIO.WriteResponse> WriteAllBytes(GIO.WriteRequest request, ServerCallContext context)
        {
            System.IO.File.WriteAllBytes(request.Path, request.Bytes.ToByteArray());
            return new GIO.WriteResponse() { Success = true };
        }
    }
}
