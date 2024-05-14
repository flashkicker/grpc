namespace COS.SDK
{
    // Define your wrapper class
    [AttributeUsage(AttributeTargets.Class)]
    public class COSModuleAttribute : Attribute
    {
        // Private fields to store module name and event emitter name
        private string _moduleName;
        private string _eventEmitterName;

        // Constructor to initialize module name and event emitter name
        public COSModuleAttribute(string moduleName = null, string eventEmitterName = null)
        {
            _moduleName = moduleName;
            _eventEmitterName = eventEmitterName;
        }

        // Property to get or set the module name
        public string ModuleName
        {
            get => _moduleName;
            set => _moduleName = value;
        }

        // Property to get or set the event emitter name
        public string EventEmitterName
        {
            get => _eventEmitterName;
            set => _eventEmitterName = value;
        }
    }
}
