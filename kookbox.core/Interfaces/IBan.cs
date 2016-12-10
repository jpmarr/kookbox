using System;

namespace kookbox.core.Interfaces
{
    public interface IBan
    {
        IUser Initiator { get; }
        DateTimeOffset ExpiryTimestamp { get; }
        Option<string> Message { get; }
    }
}
