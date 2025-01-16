// See https://aka.ms/new-console-template for more information
using Basics;
using Grpc.Core;
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
//ClientStreaming(client);
ServerStreaming(client);
//BiDirectionalStreaming(client);

Console.ReadLine();
void Unary(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    var request = new Request() { Content = "Hello you!" };
    var response = client.Unary(request, deadline: DateTime.UtcNow.AddMilliseconds(3));
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

async void ServerStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    try
    {
        var cancellationToken = new CancellationTokenSource();
        using var streamingCall = client.ServerStream(new Request() { Content = "Hello!" });

        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync(cancellationToken.Token))
        {
            Console.WriteLine(response.Message);
            if(response.Message.Contains("2"))
            {
                cancellationToken.Cancel();
            }
        }
    }

    catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
    {

    }
}

async void BiDirectionalStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    using (var call = client.BiDirectionalStream())
    {
        var request = new Request();
        for (var i = 0; i < 10; i++)
        {
            request.Content = i.ToString();
            Console.WriteLine(request.Content);
            await call.RequestStream.WriteAsync(request);
        }

        while(await call.ResponseStream.MoveNext())
        {
            var message = call.ResponseStream.Current;
            Console.WriteLine(message);
        }

        await call.RequestStream.CompleteAsync();
    }
}
