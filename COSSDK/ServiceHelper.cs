using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace COS.SDK
{
    public class ServiceNameAttribute : System.Attribute
    {
        public ServiceNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }

    public static class ServiceHelper
    {
        /// <summary>
        /// Retrieves the service name from a type annotated with the ServiceNameAttribute.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve the name from.</param>
        /// <returns>The name of the service as specified in the ServiceNameAttribute.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service type does not have a ServiceNameAttribute.</exception>
        public static string GetServiceName(Type serviceType)
        {
            // Check if the type is null to prevent null reference exception
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType), "The service type cannot be null.");
            }

            // Retrieve the ServiceName attribute from the type
            ServiceNameAttribute serviceNameAttr = serviceType.GetCustomAttribute<ServiceNameAttribute>();

            // Throw an exception if the attribute is not found
            if (serviceNameAttr == null)
            {
                throw new InvalidOperationException($"The ServiceName attribute must be defined on the {serviceType.Name} class.");
            }

            // Return the Name property of the ServiceName attribute
            return serviceNameAttr.Name;
        }
    }
}
