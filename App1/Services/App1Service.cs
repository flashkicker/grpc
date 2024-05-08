using COS.SDK;
using Grpc.Core;
using GRPC1;
using System.Threading.Tasks;

namespace App1.Services
{
    [COSModule]
    [ServiceName("GRPC1.GRPC1Service")]
    public class App1Service : GRPC1Service.GRPC1ServiceBase
    {
        [COSMethod]
        public override Task<MessageReply> SendMessage(MessageRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MessageReply { Message = "App1 just received a message: " + request.Message });
        }
    }
}
