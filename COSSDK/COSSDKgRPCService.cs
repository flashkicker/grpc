using COSSDKProto;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace COS.SDK
{
    [COSModule]
    public class COSSDKgRPCService : COSSDKService.COSSDKServiceBase
    {
        [COSMethod]
        public override async Task<COSSDKGRPCResult> GRPCExecute(COSSDKGRPCRequest request, ServerCallContext context)
        {
            var args = JsonConvert.DeserializeObject<object[]>(request.JsonArgs);
            var result = await COSSDK.CallAsync($"{request.ClassName}.{request.MethodName}" , args);
            var jsonResults = JsonConvert.SerializeObject(result);
            return new COSSDKGRPCResult { JsonResult = jsonResults };
            //var req = request.Message;
            ////deserialize
            //var arg1 = "Hello";
            //var arg2 = "World";
            //var args = new object[] { arg1, arg2 };
            //return new COSSDKGRPCResult(); //{ Message = (string)await COSSDK.CallAsync("HelloWorld.SayHello", args) };
            //return new MessageReply { Message = "App2 just received a message: " + request.Message };
        }
    }


    //MessageRequest
    //methodName e..g print
    //arguments which be string serialized {docid, doc}
}
