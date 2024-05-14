namespace COS.SDK
{
    // Define your wrapper class
    [AttributeUsage(AttributeTargets.Method)]
    public class COSMethodAttribute : Attribute
    {
        // Private field to store method name
        private string _methodName;

        // Constructor to initialize method name
        public COSMethodAttribute(string methodName = null)
        {
            _methodName = methodName;
        }

        // Property to get or set the method name
        public string MethodName
        {
            get => _methodName;
            set => _methodName = value;
        }
    }
}
