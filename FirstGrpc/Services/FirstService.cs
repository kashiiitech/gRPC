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
    }
}
