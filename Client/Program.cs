﻿// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

// creating gRPC channel
using var channel = GrpcChannel.ForAddress("https://localhost:7226");