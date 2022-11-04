namespace CompatibilityChecker.Library
{
    using System.Linq;
    using CompatibilityChecker.Library.Descriptors;

    public class Message
    {
        private readonly CompatibilityDescriptor mDescriptor;
        private readonly (string key, string value)[] mArguments;

        internal Message(CompatibilityDescriptor descriptor, params (string key, string value)[] arguments)
        {
            mDescriptor = descriptor;
            mArguments = arguments;
        }

        internal Severity Severity { get => mDescriptor.DefaultSeverity; }

        internal (string key, string value)[] Arguments { get => mArguments; }

        internal CompatibilityDescriptor Descriptor { get => mDescriptor; }

        public override string ToString()
        {
            
            string message = string.Format(mDescriptor.MessageFormat, mArguments.Select(kvp => kvp.value).ToArray());
            return string.Format("{0} {1}: {2}", mDescriptor.DefaultSeverity, mDescriptor.RuleId, message);
        }
    }
}
