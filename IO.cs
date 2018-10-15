using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using IO;

namespace Dirt.IO
{
    public static class File : IO.FileBase
    {
        public override async Task<WriteResponse> WriteAllBytes(WriteRequest request, ServerCallContext context)
        {

        }

        public static void WriteAllBytes (string path, byte[] bytes) {


            System.IO.File.WriteAllBytes(path, bytes);
        }

        public static byte[] ReadAllBytes(string path) {
            
            return System.IO.File.ReadAllBytes(path);
        }
    }
}
