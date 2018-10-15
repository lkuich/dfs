#!/bin/bash

GRPC=/home/aod/.nuget/packages/grpc.tools/1.15.0/tools/linux_x64
$GRPC/protoc -I protos --csharp_out protos/gen protos/io.proto --grpc_out protos/gen --plugin=protoc-gen-grpc=$GRPC/grpc_csharp_plugin