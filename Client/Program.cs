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
//Unary(client);
ClientStreaming(client);

Console.ReadLine();
void Unary(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    var request = new Request() { Content = "Hello you!" };
    var response = client.Unary(request);
}

async void ClientStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    using var call = client.ClientStream(); 
    for(var i = 0; i < 1000; i++)
    {
        await call.RequestStream.WriteAsync(new Request() { Content=i.ToString() });
    }

    await call.RequestStream.CompleteAsync();
    Response response = await call;
    Console.WriteLine($"{response.Message}");
}
