using COS.SDK;
using Grpc.Core;
using GRPC2;
using System.Threading.Tasks;

namespace App2.Services
{
    [COSModule]
    [ServiceName("GRPC2.GRPC2Service")]
    public class App2Service : GRPC2Service.GRPC2ServiceBase
    {
        [COSMethod]
        public override async Task<MessageReply> SendMessage(MessageRequest request, ServerCallContext context)
        {
            var arg1 = "Hello";
            var arg2 = "World";
            var args = new object[] { arg1, arg2 };
            return new MessageReply { Message = (string)await COSSDK.CallAsync("HelloWorld.SayHello", args) };
            //return Task.FromResult(new MessageReply { Message = "App2 just received a message: " + request.Message });
        }
    }
}
