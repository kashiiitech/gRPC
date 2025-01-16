using Basics;
using Grpc.Core;

namespace FirstGrpc.Services
{
    public class FirstService : FirstServiceDefinition.FirstServiceDefinitionBase
    {
        public override Task<Response> Unary(Request request, ServerCallContext context)
        {
            var response = new Response() { Message = request.Content + " from server" };
            return Task.FromResult(response);
        }

        public override async Task<Response> ClientStream(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            var response = new Response() { Message = "I got" } ;

            // read the stream from requestStream till it is available
            while(await requestStream.MoveNext())
            {
                var requestPayload = requestStream.Current;
                Console.WriteLine(requestPayload);
                response.Message = requestPayload.ToString();
            }
            return response;
        }

        public override async Task ServerStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 100; i++)
            {
                var response = new Response() { Message = i.ToString() };
                await responseStream.WriteAsync(response);
            }
        }
    }
}
