// See https://aka.ms/new-console-template for more information
using Basics;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

var option = new GrpcChannelOptions()
{

};

// creating gRPC channel
using var channel = GrpcChannel.ForAddress("https://localhost:7226", option);
// creating a client
var client = new FirstServiceDefinition.FirstServiceDefinitionClient(channel);
Unary(client);

Console.ReadLine();
void Unary(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    var request = new Request() { Content = "Hello you!" };
    var response = client.Unary(request);
}
