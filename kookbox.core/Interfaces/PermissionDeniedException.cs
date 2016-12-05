using System;

namespace kookbox.core.Interfaces
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException()
        {
        }

        public PermissionDeniedException(string message) : base(message)
        {
        }

        public PermissionDeniedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
