using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using GrpcDotNetNamedPipes;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using COSSDKProto;

namespace COS.SDK
{
    internal static class COSSDKProto
    {
        public static async Task<object> CallAsync(string key, object[] args = null)
        {
                var argMessage = (IMessage)args[0]; // Assuming the first argument is the request message
                var expectedType = (System.Type)args[1];
                var splitArray = key.Split('.');
                var channelName = string.Join(".", splitArray.Except(new List<string> { splitArray.Last() }));
                var methodName = splitArray.Last();
                var channel = new NamedPipeChannel(".", channelName);
                var serviceName = string.Join(".", splitArray.Except(new List<string> { splitArray.First(), splitArray.Last() }));


            // Create a method descriptor dynamically
            var method = new Method<IMessage, IMessage>(
                    type: MethodType.Unary,
                    serviceName: serviceName,
                    name: methodName,
                    requestMarshaller: Marshallers.Create(
                        serializer: arg => arg.ToByteArray(),
                        deserializer: bytes => argMessage.Descriptor.Parser.ParseFrom(bytes)),
                    responseMarshaller: Marshallers.Create(
                        serializer: arg => arg.ToByteArray(),
                        deserializer: bytes => ReadMessage(expectedType, bytes)
                ));

                // Make the call
                var result = await channel.AsyncUnaryCall(method, ".", new CallOptions(), argMessage);

                return result;
        }

        public static IMessage ReadMessage(System.Type expectedType, byte[] data)
        {
            if (expectedType == null)
                throw new ArgumentNullException(nameof(expectedType));
            if (!typeof(IMessage).IsAssignableFrom(expectedType))
                throw new ArgumentException("Type must implement IMessage.", nameof(expectedType));

            // Create an instance of the expected type using reflection
            IMessage message = (IMessage)Activator.CreateInstance(expectedType);

            // Merge the data from the stream into the created message object
            message.MergeFrom(data);

            return message;
        }

        public static object CallSync(string key, object[] args = null)
        {
            var argMessage = (IMessage)args[0]; // Assuming the first argument is the request message
            var expectedType = (System.Type)args[1];
            var splitArray = key.Split('.');
            var serviceName = string.Join(".", splitArray.Except(new List<string> { splitArray.Last() }));
            var methodName = splitArray.Last();
            var channel = new NamedPipeChannel(".", serviceName);

            // Create a method descriptor dynamically
            var method = new Method<IMessage, IMessage>(
                type: MethodType.Unary,
                serviceName: serviceName,
                name: methodName,
                requestMarshaller: Marshallers.Create(
                    serializer: arg => arg.ToByteArray(),
                    deserializer: bytes => argMessage.Descriptor.Parser.ParseFrom(bytes)),
                responseMarshaller: Marshallers.Create(
                    serializer: arg => arg.ToByteArray(),
                    deserializer: bytes => ReadMessage(expectedType, bytes)
            ));

            // Make the call synchronously
            var result = channel.BlockingUnaryCall(method, ".", new CallOptions(), argMessage);

            return result;
        }
    }
}
