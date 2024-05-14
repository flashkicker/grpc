using COSSDKProto;
using Google.Protobuf;
using Grpc.Core;
using GrpcDotNetNamedPipes;
using System.Reflection;

namespace COS.SDK
{

    public class COSSDK
    {
        static readonly Dictionary<string, MethodInfo> COSAsyncMethods = new Dictionary<string, MethodInfo>();
        static readonly Dictionary<string, MethodInfo> COSSyncMethods = new Dictionary<string, MethodInfo>();

        static COSSDK()
        {
            ProcessAssemblies();
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
        }

        private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            // An assembly has been loaded
            Console.WriteLine("Assembly loaded: " + args.LoadedAssembly.FullName);

            // Call ProcessAssembly to process the newly loaded assembly
            ProcessAssembly(args.LoadedAssembly);
        }

        public static void ProcessTypes(Type[] types)
        {
            var typesWithMyAttribute = types.Where(t => t.GetCustomAttributes(typeof(COSModuleAttribute), true).Length > 0);

            // Print the names of the types
            foreach (var type in typesWithMyAttribute)
            {
                // Create an instance of the type
                object instance = Activator.CreateInstance(type);

                // Get the methods that have the COSMethod attribute
                var methodsWithMyAttribute = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(COSMethodAttribute), true).Length > 0);

                // For each method with the COSMethod attribute
                foreach (var method in methodsWithMyAttribute)
                {
                    string key = $"{type.Name}.{method.Name}";

                    // Check if the method has already been added to the dictionaries
                    if (!COSAsyncMethods.ContainsKey(key))
                    {
                        COSAsyncMethods.Add(key, method);
                    }

                    if (!COSSyncMethods.ContainsKey(key))
                    {
                        COSSyncMethods.Add(key, method);
                    }
                }
            }
        }

        public static void ProcessAssembly(Assembly assembly)
        {
            // Get all types
            Type[] types = assembly.GetTypes();

            // Process the types
            ProcessTypes(types);
        }

        public static void ProcessAssemblies()
        {
            // Get all currently loaded assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                // Get all types
                Type[] types = assembly.GetTypes();

                // Process the types
                ProcessTypes(types);
            }
        }

        public static async Task<TReturn> CallAsync<TReturn>(string serviceName, string methodName, object[] functionArguments)
            //where TInput : IMessage
            //where TReturn : IMessage
        {
            var jsonArgs = Newtonsoft.Json.JsonConvert.SerializeObject(functionArguments);
            var args = new object[] { new COSSDKGRPCRequest() { ClassName = "HelloWorld", MethodName = "SayHello", JsonArgs = Newtonsoft.Json.JsonConvert.SerializeObject(new object[] { "str1", "str2" }) }, typeof(COSSDKGRPCResult) };

            var result = await COSSDK.CallAsync($"{serviceName}.COSSDKProto.COSSDKService.{methodName}", args.ToArray());

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TReturn>(((COSSDKGRPCResult)result).JsonResult);
        }

        public static async Task<object> CallAsync(string key, object[] args = null)
        {
            // Check if the key exists in the dictionary
            if (COSAsyncMethods.ContainsKey(key))
            {
                // Get the MethodInfo from the dictionary
                MethodInfo method = COSAsyncMethods[key];

                // Get the class type
                Type classType = method.DeclaringType;

                // Create an instance of the class
                object classInstance = Activator.CreateInstance(classType);

                // If args is null, initialize it as an empty object array
                if (args == null)
                {
                    args = new object[0];
                }

                // Use Task.Run to call the method asynchronously
                var result = await Task.Run(() =>
                {
                    try
                    {
                        // Invoke the method and return the result
                        return method.Invoke(classInstance, args);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Console.WriteLine(ex);
                        throw;
                    }
                });

                // If the method returns a Task<T>, extract the result of the Task<T>
                if (result is Task task)
                {
                    if (task.GetType().IsGenericType)
                    {
                        return ((dynamic)task).Result;
                    }
                    await task;
                    return null;
                }

                return result;
            }
            else
            {
                return await COSSDKProto.CallAsync(key, args);
            }
            //else
            //{
            //    throw new Exception($"No method found with the key: {key}");
            //}
        }

        public static TReturn CallSync<TInput, TReturn>(string serviceName, string methodName, TInput functionArgument)
            where TInput : IMessage
            where TReturn : IMessage
        {
            var args = new object[] { functionArgument, typeof(TReturn) };
            return (TReturn)COSSDK.CallSync($"{serviceName}.{methodName}", args);
        }

        public static object CallSync(string key, object[] args = null)
        {
            // Check if the key exists in the dictionary
            if (COSSyncMethods.ContainsKey(key))
            {
                // Get the MethodInfo from the dictionary
                MethodInfo method = COSSyncMethods[key];

                // Get the class type
                Type classType = method.DeclaringType;

                // Create an instance of the class
                object classInstance = Activator.CreateInstance(classType);

                // If args is null, initialize it as an empty object array
                if (args == null)
                {
                    args = new object[0];
                }

                return method.Invoke(classInstance, args);
            }
            else
            {
                return COSSDKProto.CallSync(key, args);
            }
        }

        public static void InitService(string moduleName)
        {
            var server = new NamedPipeServer($"{moduleName}.COSSDKProto.COSSDKService");
            COSSDKService.BindService(server.ServiceBinder, new COSSDKgRPCService());
            server.Start();
        }
    }
}
