using System;

namespace kookbox.core.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisteredPayloadAttribute : Attribute
    {
        public RegisteredPayloadAttribute(short messageType, byte version)
        {
            MessageType = messageType;
            Version = version;
        }

        public short MessageType { get; }
        public byte Version { get;  }
    }
}
